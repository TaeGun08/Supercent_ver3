using System.Collections;
using UnityEngine;
using DG.Tweening;
using Project.Core.Interfaces;

public enum PrisonerState { MovingToTableLine, WaitingInTableLine, TakingPotions, MovingToPrison, WaitingForPrisonSpace }

public class Prisoner : Npc
{
    [Header("Prisoner Settings")]
    [SerializeField] private GameObject headObject;
    
    private int requiredPotionCount;
    private int currentPotionCount = 0;
    private PotionTable targetTable;
    private bool isUIShown;
    
    // 상태 객체 캐싱
    private INpcState stateToTableLine;
    private INpcState stateWaitingLine;
    private INpcState stateTakingPotion;
    private INpcState stateToPrison;
    private INpcState stateWaitingPrison;

    private WaitForSeconds wait;

    protected override void Awake()
    {
        base.Awake();
        
        wait = new WaitForSeconds(0.2f);
        if (headObject != null) headObject.SetActive(false);

        InitializeStates();
    }

    private void InitializeStates()
    {
        stateToTableLine = new State_ToTableLine(this);
        stateWaitingLine = new State_WaitingLine(this);
        stateTakingPotion = new State_TakingPotion(this);
        stateToPrison = new State_ToPrison(this);
        stateWaitingPrison = new State_WaitingPrison(this);
    }

    public void Initialize(PotionTable table, int neededPotions)
    {
        targetTable = table;
        requiredPotionCount = neededPotions;
        currentPotionCount = 0;
        isUIShown = false;
        
        if (headObject != null) headObject.SetActive(false);
        
        ChangeState(stateToTableLine);
    }

    #region Helper Methods (Feedback & Logic)

    public bool IsAtTableSpot()
    {
        if (WaitingLineManager.Instance.GetFrontNpc() != this || IsMoving) return false;
        Vector3 tablePos = WaitingLineManager.Instance.GetPoint(0);
        return Vector3.Distance(transform.position, tablePos) < 0.2f;
    }

    public void ShowUI()
    {
        if (isUIShown) return;
        UIManager.Instance.ShowPrisonerUI(transform, currentPotionCount, requiredPotionCount);
        isUIShown = true;
    }

    public void HideUI()
    {
        UIManager.Instance.HidePrisonerUI();
        isUIShown = false;
    }

    public void UpdateUI() => UIManager.Instance.UpdatePrisonerUI(currentPotionCount);

    public bool CanTakePotion() => IsAtTableSpot() && targetTable != null && targetTable.CanDistribute;

    public void CompleteCollection()
    {
        HideUI();
        AudioManager.Instance.Play(SoundType.PrisonerFull);

        if (headObject != null)
        {
            headObject.SetActive(true);
            EffectManager.Instance.PlayEffect("DistributionSuccess", headObject.transform.position);
        }

        if (targetTable != null) targetTable.ProduceGold();
        WaitingLineManager.Instance.OnFrontPersonLeft();
    }

    public IEnumerator ExecuteTakingPotionRoutine()
    {
        ItemStacker source = targetTable.GetStacker();
        IPickupAble potion = source.PopStack();
        if (potion == null) yield break;

        AudioManager.Instance.Play(SoundType.PotionDistributed);
        bool moveComplete = false;
        
        DOParabolicMove.MoveToDynamicTarget(
            potion.Transform,
            transform, 
            height: 1.5f,
            duration: 0.2f,
            localOffset: Vector3.up * 1.0f, 
            endRotY: 0f,
            onComplete: () =>
            {
                currentPotionCount++;
                UpdateUI();
                potion.Release(); 
                moveComplete = true;
            }
        );

        yield return new WaitUntil(() => moveComplete);
        yield return wait;
    }

    #endregion

    #region Concrete States

    private class State_ToTableLine : INpcState
    {
        private Prisoner p;
        public State_ToTableLine(Prisoner prisoner) => p = prisoner;
        public void Enter() => WaitingLineManager.Instance.JoinLine(p);
        public void Execute() { if (!p.IsMoving) p.ChangeState(p.stateWaitingLine); }
        public void Exit() { }
    }

    private class State_WaitingLine : INpcState
    {
        private Prisoner p;
        public State_WaitingLine(Prisoner prisoner) => p = prisoner;
        public void Enter() { }
        public void Execute()
        {
            if (p.IsAtTableSpot()) p.ShowUI();

            if (p.currentPotionCount >= p.requiredPotionCount)
            {
                p.ChangeState(p.stateToPrison);
                return;
            }

            if (p.CanTakePotion()) p.ChangeState(p.stateTakingPotion);
        }
        public void Exit() { }
    }

    private class State_TakingPotion : INpcState
    {
        private Prisoner p;
        private bool isProcessing;
        public State_TakingPotion(Prisoner prisoner) => p = prisoner;
        public void Enter() => isProcessing = false;
        public void Execute()
        {
            if (isProcessing) return;
            p.StartCoroutine(ProcessTaking());
        }
        private IEnumerator ProcessTaking()
        {
            isProcessing = true;
            yield return p.StartCoroutine(p.ExecuteTakingPotionRoutine());
            isProcessing = false;
            
            // 수령 완료 체크 후 상태 전이
            if (p.currentPotionCount >= p.requiredPotionCount) p.ChangeState(p.stateToPrison);
            else p.ChangeState(p.stateWaitingLine);
        }
        public void Exit() { }
    }

    private class State_ToPrison : INpcState
    {
        private Prisoner p;
        public State_ToPrison(Prisoner prisoner) => p = prisoner;
        public void Enter() 
        {
            p.CompleteCollection();
            p.MoveTo(PrisonManager.Instance.GetWaitingPoint());
        }
        public void Execute() { if (!p.IsMoving) p.ChangeState(p.stateWaitingPrison); }
        public void Exit() { }
    }

    private class State_WaitingPrison : INpcState
    {
        private Prisoner p;
        public State_WaitingPrison(Prisoner prisoner) => p = prisoner;
        public void Enter() => PrisonManager.Instance.TryEnterPrison(p);
        public void Execute() { }
        public void Exit() { }
    }

    #endregion
}
