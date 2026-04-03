using System.Collections;
using UnityEngine;

public enum TransportState { ToCauldron, Loading, ToTable, Unloading, Distributing }

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
    private TransportState currentState;
    private WaitForSeconds wait = new WaitForSeconds(0.3f);

    protected override void Awake()
    {
        base.Awake();
        targetCauldron = FindObjectOfType<MagicCauldron>();
        targetTable = FindObjectOfType<PotionTable>();
    }

    private void Start()
    {
        StartCoroutine(TransportRoutine());
    }

    private IEnumerator TransportRoutine()
    {
        while (true)
        {
            // 1. 가마솥으로 이동
            currentState = TransportState.ToCauldron;
            MoveTo(cauldronPoint.position);
            yield return new WaitUntil(() => !IsMoving);

            // 2. 포션 수집 (내 인벤토리가 꽉 차거나 가마솥이 비기 전까지)
            currentState = TransportState.Loading;
            ItemStacker cauldronOutput = targetCauldron.GetOutputStacker();
            while (cauldronOutput.HasItem && !myStacker.IsFull)
            {
                IPickupAble item = cauldronOutput.PopStack();
                if (item != null)
                {
                    yield return StartCoroutine(CollectItem(item));
                }
            }

            // 3. 테이블로 이동
            currentState = TransportState.ToTable;
            MoveTo(tablePoint.position);
            yield return new WaitUntil(() => !IsMoving);

            // 4. 포션 내려놓기
            currentState = TransportState.Unloading;
            ItemStacker tableInput = targetTable.GetStacker();
            while (myStacker.HasItem && !tableInput.IsFull)
            {
                IPickupAble item = myStacker.PopStack();
                if (item != null)
                {
                    yield return StartCoroutine(DepositItem(item, tableInput));
                }
            }

            // 5. 테이블 상주 및 배급 지원 (운반할 게 없고 테이블이 비어있지 않을 때)
            if (!myStacker.HasItem)
            {
                currentState = TransportState.Distributing;
                MoveTo(distributionStandPoint.position);
                yield return new WaitUntil(() => !IsMoving);
                
                // 테이블에 상주하며 플레이어 대신 배급 상태 유지 (조건부 대기)
                while (tableInput.HasItem && !cauldronOutput.HasItem)
                {
                    yield return wait;
                }
            }
            
            yield return null;
        }
    }

    private IEnumerator CollectItem(IPickupAble item)
    {
        bool complete = false;
        DOParabolicMove.MoveToDynamicTarget(
            item.Transform,
            myStacker.transform,
            height: 1.5f,
            duration: 0.15f,
            yOffset: myStacker.CurrentCount * myStacker.ItemHeight,
            onComplete: () => {
                myStacker.PushStack(item);
                complete = true;
            }
        );
        yield return new WaitUntil(() => complete);
    }

    private IEnumerator DepositItem(IPickupAble item, ItemStacker target)
    {
        bool complete = false;
        Vector3 targetPos = target.transform.position + target.GetNextLocalPosition();
        DOParabolicMove.MoveToStaticPosition(
            item.Transform,
            targetPos,
            height: 1.5f,
            duration: 0.15f,
            onComplete: () => {
                target.PushStack(item);
                complete = true;
            }
        );
        yield return new WaitUntil(() => complete);
    }

    public bool IsAtDistributionPoint() 
    {
        return currentState == TransportState.Distributing && !IsMoving;
    }
}
