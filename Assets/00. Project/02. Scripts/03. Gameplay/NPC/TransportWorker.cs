using System.Collections;
using DG.Tweening;
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
    private WaitForSeconds wait;

    protected override void Awake()
    {
        base.Awake();
        wait = new WaitForSeconds(0.3f);
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
            currentState = TransportState.ToCauldron;
            MoveTo(cauldronPoint.position);
            yield return new WaitUntil(() => !IsMoving);

            currentState = TransportState.WaitingAtCauldron;
            ItemStacker cauldronOutput = targetCauldron.GetOutputStacker();
            
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

            currentState = TransportState.ToTable;
            MoveTo(tablePoint.position);
            yield return new WaitUntil(() => !IsMoving);
            
            transform.DORotate(new Vector3(0, 180f, 0), 0.3f);

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

            if (tableInput.HasItem)
            {
                currentState = TransportState.Distributing;
                MoveTo(distributionStandPoint.position);
                yield return new WaitUntil(() => !IsMoving);
                
                transform.DORotate(new Vector3(0, 180f, 0), 0.3f);

                while (tableInput.HasItem)
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
