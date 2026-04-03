using System.Collections;
using UnityEngine;

public enum TransportState { ToCauldron, WaitingAtCauldron, ToTable, UnloadingAtTable, Distributing }

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
        if (targetCauldron == null || targetTable == null) return;
        StartCoroutine(TransportRoutine());
    }

    private IEnumerator TransportRoutine()
    {
        while (true)
        {
            // 1. 가마솥으로 이동 및 완충 대기
            currentState = TransportState.ToCauldron;
            MoveTo(cauldronPoint.position);
            yield return new WaitUntil(() => !IsMoving);

            currentState = TransportState.WaitingAtCauldron;
            ItemStacker cauldronOutput = targetCauldron.GetOutputStacker();
            
            // [Strict Loading]: 내 스택이 꽉 찰 때까지 무한 대기
            while (!myStacker.IsFull)
            {
                if (cauldronOutput.HasItem)
                {
                    IPickupAble item = cauldronOutput.PopStack();
                    if (item != null)
                    {
                        yield return StartCoroutine(CollectItem(item));
                    }
                }
                yield return wait;
            }

            // 2. 테이블로 이동
            currentState = TransportState.ToTable;
            MoveTo(tablePoint.position);
            yield return new WaitUntil(() => !IsMoving);

            // 3. 하적 (내 스택이 완전히 비워질 때까지)
            currentState = TransportState.UnloadingAtTable;
            ItemStacker tableInput = targetTable.GetStacker();
            while (myStacker.HasItem)
            {
                if (!tableInput.IsFull)
                {
                    IPickupAble item = myStacker.PopStack();
                    if (item != null)
                    {
                        yield return StartCoroutine(DepositItem(item, tableInput));
                    }
                }
                yield return wait;
            }

            // 4. 테이블 검사 및 배급 결정
            if (tableInput.HasItem)
            {
                // [Distribution Phase]: 테이블에 포션이 있는 동안 상주하며 배급
                currentState = TransportState.Distributing;
                MoveTo(distributionStandPoint.position);
                yield return new WaitUntil(() => !IsMoving);

                // 테이블의 포션이 다 떨어질 때까지 대기 (배급 지원)
                while (tableInput.HasItem)
                {
                    yield return wait;
                }
            }
            
            // 배급 완료 혹은 테이블이 비어있으면 다시 가마솥으로 (Loop)
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
