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
    
    [Header("Dependencies")]
    [SerializeField] private MagicCauldron targetCauldron;
    [SerializeField] private PotionTable targetTable;

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

    private void Start()
    {
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
        public void Execute() 
        { 
            // 가마솥 포인트 도착 시 수집 상태로
            if (!worker.IsMoving) worker.ChangeState(worker.stateLoading); 
        }
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
            
            // 1. 최대치만큼 챙기면 테이블로 이동
            if (worker.myStacker.IsFull) 
            { 
                worker.ChangeState(worker.stateToTable); 
                return; 
            }

            // 2. 가마솥에 물건이 있으면 계속 수집
            ItemStacker output = worker.targetCauldron.GetOutputStacker();
            if (output.HasItem)
            {
                IPickupAble item = output.PopStack();
                if (item != null) worker.StartCoroutine(HandleCollect(item));
            }
            else if (worker.myStacker.HasItem)
            {
                // 가마솥이 비었더라도 들고 있는 게 있다면 일단 테이블로 운반 (유동적 흐름)
                worker.ChangeState(worker.stateToTable);
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
                // 도착 즉시 하적 상태로
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

            // 1. 자신의 인벤토리가 다 비워졌다면
            if (!worker.myStacker.HasItem)
            {
                // 테이블에 포션이 있다면 배급 대기, 없다면 다시 가지러 감
                if (worker.targetTable.GetStacker().HasItem) worker.ChangeState(worker.stateDistributing);
                else worker.ChangeState(worker.stateToCauldron);
                return;
            }

            // 2. 테이블 인벤토리가 가득 찼다면 (자신은 남았어도 더 이상 못 쌓음)
            ItemStacker tableInput = worker.targetTable.GetStacker();
            if (tableInput.IsFull)
            {
                worker.ChangeState(worker.stateDistributing);
                return;
            }

            // 3. 하적 로직 수행
            IPickupAble item = worker.myStacker.PopStack();
            if (item != null) worker.StartCoroutine(HandleDeposit(item, tableInput));
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
        public void Enter() 
        {
            worker.MoveTo(worker.distributionStandPoint.position, () => {
                // 도착 시 고정된 방향(뒤쪽)을 바라봄
                worker.transform.DORotate(new Vector3(0, 180f, 0), 0.3f);
            });
        }
        public void Execute()
        {
            if (worker.IsMoving) return;
            
            // 테이블의 포션이 다 사라졌다면 다시 가지러 가기
            if (!worker.targetTable.GetStacker().HasItem)
            {
                worker.ChangeState(worker.stateToCauldron);
            }
        }
        public void Exit() { }
    }

    #endregion
}
