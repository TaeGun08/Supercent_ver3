using System.Collections;
using UnityEngine;
using DG.Tweening;

public enum PrisonerState { MovingToTableLine, WaitingInTableLine, TakingPotions, MovingToPrison, WaitingForPrisonSpace }

public class Prisoner : Npc
{
    [Header("Prisoner Settings")]
    [SerializeField] private GameObject headObject;
    
    private int requiredPotionCount;
    private int currentPotionCount = 0;
    private PrisonerState currentState;
    private PotionTable targetTable;
    private bool isUIShown;
    
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
        currentPotionCount = 0;
        isUIShown = false;
        
        StartCoroutine(MainBehaviorRoutine());
    }
private IEnumerator MainBehaviorRoutine()
{
    yield return StartCoroutine(EnterTableWaitingLine());

    // [UI Activation]: 테이블 앞 대기 지점 도착 즉시 UI 노출 (배급 시작 전)
    if (WaitingLineManager.Instance.GetFrontNpc() == this && !IsMoving)
    {
        UIManager.Instance.ShowPrisonerUI(transform, currentPotionCount, requiredPotionCount);
        isUIShown = true;
    }

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
        // [UI Refresh]: 전진 등으로 인해 맨 앞이 되었는데 UI가 없다면 즉시 켜줌
        if (!isUIShown && WaitingLineManager.Instance.GetFrontNpc() == this && !IsMoving)
        {
            UIManager.Instance.ShowPrisonerUI(transform, currentPotionCount, requiredPotionCount);
            isUIShown = true;
        }

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
        
        if (isUIShown)
        {
            UIManager.Instance.HidePrisonerUI();
            isUIShown = false;
        }

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
                if (isUIShown)
                {
                    UIManager.Instance.UpdatePrisonerUI(currentPotionCount);
                }

                potion.Release(); 
                moveComplete = true;
            }
        );

        yield return new WaitUntil(() => moveComplete);
        yield return wait;
    }
}