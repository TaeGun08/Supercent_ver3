using System.Collections;
using UnityEngine;

public enum PrisonerState { MovingToTableLine, WaitingInTableLine, TakingPotions, MovingToPrison, WaitingForPrisonSpace }

public class Prisoner : Npc
{
    [Header("Prisoner Settings")]
    [SerializeField] private GameObject headObject;
    
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
        
        yield return new WaitUntil(() => !IsMoving);
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
               && IsMoving == false
               && targetTable != null 
               && targetTable.CanDistribute;
    }

    private void CompleteCollection()
    {
        if (headObject != null) headObject.SetActive(true);
        WaitingLineManager.Instance.OnFrontPersonLeft();
    }

    private IEnumerator GoToPrison()
    {
        currentState = PrisonerState.MovingToPrison;
        PrisonManager.Instance.TryEnterPrison(this);
        yield break; 
    }

    private IEnumerator ExecuteTakingPotion(ItemStacker source)
    {
        IPickupAble potion = source.PopStack();
        if (potion == null) yield break;
        currentPotionCount++;

        yield return wait;
    }
}