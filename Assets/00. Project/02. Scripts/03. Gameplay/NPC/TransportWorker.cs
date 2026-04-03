using System.Collections;
using DG.Tweening;
using UnityEngine;
using Project.Core.Interfaces;

public class TransportWorker : Npc
{
    [Header("Route Settings")]
    [SerializeField] private Transform cauldronPoint;
    [SerializeField] private Transform tablePoint;
    [SerializeField] private Transform distributionStandPoint;

    [Header("Inventory Settings")]
    [SerializeField] private ItemStacker myStacker;
    
    private MagicCauldron targetCauldron;
    private PotionTable targetTable;

    // 캐싱된 상태 객체들
    private INpcState stateToCauldron;
    private INpcState stateLoading;
    private INpcState stateToTable;
    private INpcState stateUnloading;
    private INpcState stateDistributing;

    protected override void Awake()
    {
        base.Awake();
        
        // 상태 객체 초기화 (Flyweight)
        stateToCauldron = new State_ToCauldron(this);
        stateLoading = new State_Loading(this);
        stateToTable = new State_ToTable(this);
        stateUnloading = new State_Unloading(this);
        stateDistributing = new State_Distributing(this);
    }

    public void Initialize(MagicCauldron cauldron, PotionTable table)
    {
        targetCauldron = cauldron;
        targetTable = table;
        
        if (targetCauldron != null && targetTable != null)
        {
            ChangeState(stateToCauldron);
        }
    }

    public bool IsAtDistributionPoint() 
    {
        if (currentState != stateDistributing || IsMoving) return false;
        return Vector3.Distance(transform.position, distributionStandPoint.position) < 0.5f;
    }

    #region Concrete States (Concrete Strategy)

    private class State_ToCauldron : INpcState
    {
        private TransportWorker worker;
        public State_ToCauldron(TransportWorker w) => worker = w;
        public void Enter() => worker.MoveTo(worker.cauldronPoint.position);
        public void Execute() { if (!worker.IsMoving) worker.ChangeState(worker.stateLoading); }
        public void Exit() { }
    }

    private class State_Loading : INpcState
    {
        private TransportWorker worker;
        private bool isHandling;
        public State_Loading(TransportWorker w) => worker = w;
        public void Enter() => isHandling = false;
        public void Execute()
        {
            if (isHandling) return;
            if (worker.myStacker.IsFull) { worker.ChangeState(worker.stateToTable); return; }

            ItemStacker output = worker.targetCauldron.GetOutputStacker();
            if (output.HasItem)
            {
                IPickupAble item = output.PopStack();
                if (item != null) worker.StartCoroutine(HandleCollect(item));
            }
        }
        private IEnumerator HandleCollect(IPickupAble item)
        {
            isHandling = true;
            bool complete = false;
            DOParabolicMove.MoveToDynamicTarget(item.Transform, worker.myStacker.transform, 1.5f, 0.15f, 
                worker.myStacker.GetNextLocalPosition(), 0f, () => { worker.myStacker.PushStack(item); complete = true; });
            yield return new WaitUntil(() => complete);
            isHandling = false;
        }
        public void Exit() { }
    }

    private class State_ToTable : INpcState
    {
        private TransportWorker worker;
        public State_ToTable(TransportWorker w) => worker = w;
        public void Enter() => worker.MoveTo(worker.tablePoint.position);
        public void Execute() 
        { 
            if (!worker.IsMoving) 
            {
                worker.transform.DORotate(new Vector3(0, 180f, 0), 0.3f);
                worker.ChangeState(worker.stateUnloading); 
            }
        }
        public void Exit() { }
    }

    private class State_Unloading : INpcState
    {
        private TransportWorker worker;
        private bool isHandling;
        public State_Unloading(TransportWorker w) => worker = w;
        public void Enter() => isHandling = false;
        public void Execute()
        {
            if (isHandling) return;
            if (!worker.myStacker.HasItem)
            {
                if (worker.targetTable.GetStacker().HasItem) worker.ChangeState(worker.stateDistributing);
                else worker.ChangeState(worker.stateToCauldron);
                return;
            }

            ItemStacker tableInput = worker.targetTable.GetStacker();
            if (!tableInput.IsFull)
            {
                IPickupAble item = worker.myStacker.PopStack();
                if (item != null) worker.StartCoroutine(HandleDeposit(item, tableInput));
            }
        }
        private IEnumerator HandleDeposit(IPickupAble item, ItemStacker target)
        {
            isHandling = true;
            bool complete = false;
            Vector3 targetPos = target.transform.TransformPoint(target.GetNextLocalPosition());
            DOParabolicMove.MoveToStaticPosition(item.Transform, targetPos, 1.5f, 0.15f, 0f, () => { target.PushStack(item); complete = true; });
            yield return new WaitUntil(() => complete);
            isHandling = false;
        }
        public void Exit() { }
    }

    private class State_Distributing : INpcState
    {
        private TransportWorker worker;
        public State_Distributing(TransportWorker w) => worker = w;
        public void Enter() => worker.MoveTo(worker.distributionStandPoint.position);
        public void Execute()
        {
            if (worker.IsMoving) return;
            worker.transform.rotation = Quaternion.Lerp(worker.transform.rotation, Quaternion.Euler(0, 180f, 0), Time.deltaTime * worker.rotationSpeed);
            
            // 테이블이 비워지거나 가마솥에 물건이 나오면 다시 수집 모드로
            if (!worker.targetTable.GetStacker().HasItem || worker.targetCauldron.GetOutputStacker().HasItem)
            {
                worker.ChangeState(worker.stateToCauldron);
            }
        }
        public void Exit() { }
    }

    #endregion
}
