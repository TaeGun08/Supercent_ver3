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

        // [Spatial Guard]: 대기열 맨 앞이면서, 실제 테이블 위치에 안착했을 때만 UI 노출
        if (IsAtTableSpot())
        {
            UIManager.Instance.ShowPrisonerUI(transform, currentPotionCount, requiredPotionCount);
            isUIShown = true;
        }

        yield return StartCoroutine(CollectRequiredPotions());

        CompleteCollection();

        yield return StartCoroutine(GoToPrison());
    }

    private bool IsAtTableSpot()
    {
        // 대기열 맨 앞인지 + 이동 중이 아닌지 + 테이블 포인트(0번)와의 거리가 충분히 가까운지 확인
        if (WaitingLineManager.Instance.GetFrontNpc() != this || IsMoving) return false;
        
        Vector3 tablePos = WaitingLineManager.Instance.GetPoint(0);
        return Vector3.Distance(transform.position, tablePos) < 0.2f;
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
            // [Refresh Guard]: 전진 중 안착했을 때 UI 노출
            if (!isUIShown && IsAtTableSpot())
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
        return IsAtTableSpot() 
               && targetTable != null 
               && targetTable.CanDistribute;
    }

    private void CompleteCollection()
    {
        // [Unconditional Hide]: 플래그와 상관없이 UIManager에 직접 Hide 명령 (참조 무결성 확보)
        UIManager.Instance.HidePrisonerUI();
        isUIShown = false;

        AudioManager.Instance.Play(SoundType.PrisonerFull);
        if (headObject != null)
        {
            headObject.SetActive(true);
            // 배급 완료 이펙트 재생 (머리 위치)
            EffectManager.Instance.PlayEffect("DistributionSuccess", headObject.transform.position);
        }

        if (targetTable != null) targetTable.ProduceGold();

        WaitingLineManager.Instance.OnFrontPersonLeft();
    }

    private IEnumerator GoToPrison()
    {
        // [Guaranteed Hide]: 이동 시작 시 UI 확실히 제거
        if (isUIShown)
        {
            UIManager.Instance.HidePrisonerUI();
            isUIShown = false;
        }

        currentState = PrisonerState.MovingToPrison;

        // [Waiting Logic]: 감옥 대기 구역으로 이동
        Vector3 waitingPoint = PrisonManager.Instance.GetWaitingPoint();
        MoveTo(waitingPoint);

        yield return new WaitUntil(() => !IsMoving);

        // 도착 후 정렬 연출이나 대기 시간 없이 즉시 수감 시도
        currentState = PrisonerState.WaitingForPrisonSpace;
        PrisonManager.Instance.TryEnterPrison(this);
    }

    private IEnumerator ExecuteTakingPotion(ItemStacker source)
    {
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
                
                // [Sync UI]: 수령 중에는 숫자만 계속 업데이트
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
