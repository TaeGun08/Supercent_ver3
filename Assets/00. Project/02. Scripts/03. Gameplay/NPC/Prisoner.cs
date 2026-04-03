using System.Collections;
using UnityEngine;
using DG.Tweening;

public enum PrisonerState { MovingToTableLine, WaitingInTableLine, TakingPotions, MovingToPrison, WaitingForPrisonSpace }

public class Prisoner : Npc
{
    [Header("Prisoner Settings")]
    [SerializeField] private GameObject headObject;
    [SerializeField] private PrisonerUI prisonerUI;
    
    private int requiredPotionCount;
    private int currentPotionCount = 0;
    private PrisonerState currentState;
    private PotionTable targetTable;
    
    private WaitForSeconds wait;

    protected override void Awake()
    {
        base.Awake();
        
        wait = new WaitForSeconds(0.2f);
        if (headObject != null) headObject.SetActive(false);
    }

    public void Initialize(PotionTable table, int neededPotions)
    {
        targetTable = table;
        requiredPotionCount = neededPotions;
        
        StartCoroutine(MainBehaviorRoutine());
    }

    private IEnumerator MainBehaviorRoutine()
    {
        yield return StartCoroutine(EnterTableWaitingLine());
        
        yield return StartCoroutine(CollectRequiredPotions());

        CompleteCollection();

        yield return StartCoroutine(GoToPrison());
    }

    private IEnumerator EnterTableWaitingLine()
    {
        currentState = PrisonerState.MovingToTableLine;
        WaitingLineManager.Instance.JoinLine(this);
        
        yield return new WaitUntil(() => IsMoving == false);
        
        currentState = PrisonerState.WaitingInTableLine;
    }

    private IEnumerator CollectRequiredPotions()
    {
        while (currentPotionCount < requiredPotionCount)
        {
            if (CanTakePotion())
            {
                currentState = PrisonerState.TakingPotions;
                yield return StartCoroutine(ExecuteTakingPotion(targetTable.GetStacker()));
                currentState = PrisonerState.WaitingInTableLine;
            }
            yield return wait;
        }
    }

    private bool CanTakePotion()
    {
        return WaitingLineManager.Instance.GetFrontNpc() == this 
               && !IsMoving 
               && targetTable != null 
               && targetTable.CanDistribute;
    }

    private void CompleteCollection()
    {
        if (headObject != null) headObject.SetActive(true);
        if (prisonerUI != null) prisonerUI.Hide();

        if (targetTable != null) targetTable.ProduceGold();

        WaitingLineManager.Instance.OnFrontPersonLeft();
    }

    private IEnumerator GoToPrison()
    {
        currentState = PrisonerState.MovingToPrison;

        Vector3 waitingPoint = PrisonManager.Instance.GetWaitingPoint();
        MoveTo(waitingPoint);

        yield return new WaitUntil(() => !IsMoving);
        yield return new WaitForSeconds(0.3f); 

        currentState = PrisonerState.WaitingForPrisonSpace;

        PrisonManager.Instance.TryEnterPrison(this);
    }

    private IEnumerator ExecuteTakingPotion(ItemStacker source)
    {
        IPickupAble potion = source.PopStack();
        if (potion == null) yield break;

        bool moveComplete = false;
        
        DOParabolicMove.MoveToDynamicTarget(
            potion.Transform,
            transform, 
            height: 1.5f,
            duration: 0.2f,
            yOffset: 1.0f, 
            onComplete: () =>
            {
                currentPotionCount++;
                if (prisonerUI != null) prisonerUI.UpdateCount(currentPotionCount);

                potion.Release(); 
                moveComplete = true;
            }
        );

        yield return new WaitUntil(() => moveComplete);
        yield return wait;
    }
}