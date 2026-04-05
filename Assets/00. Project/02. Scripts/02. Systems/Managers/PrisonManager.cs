using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PrisonManager : SingletonBase<PrisonManager>
{
    [Header("Prison Capacity")]
    [SerializeField] private int maxCapacity = 20;
    [SerializeField] private Transform prisonEntrance;
    [SerializeField] private Transform waitingAreaPoint;
    [SerializeField] private GameObject prisonDoor;
    [SerializeField] private TMP_Text prisonerCountText;

    private int currentPrisonerCount;
    private int activePrisoners;
    private int enteringCount; 
    private readonly HashSet<Npc> waitingNPCs = new();

    // [SOLID]: 외부 노출 프로퍼티
    public int CurrentCount => currentPrisonerCount;
    public int MaxCapacity => maxCapacity;

    // [SOLID]: 상태 변화를 알리는 이벤트 (현재 인원, 최대 수용량)
    public event System.Action<int, int> OnPrisonerCountChanged;

    [Header("Tutorial Hook")]
    [SerializeField] private GameObject prisonUnlockZone;
    [SerializeField] private Transform prisonUnlockTarget;
    private bool hasTriggeredExpandGuide;

    public bool IsFull => activePrisoners >= maxCapacity;
    public bool IsPrisonFull => currentPrisonerCount >= maxCapacity;
    
    public void ExpandCapacity(int additionalAmount)
    {
        maxCapacity += additionalAmount;
        hasTriggeredExpandGuide = false; // 확장 후에는 다시 트리거 가능
        
        NotifyStateChanged();
        
        // 확장 즉시 대기열에 있던 죄수들을 가용량만큼 수용 시도
        RefreshWaitingQueue();
    }

    public void RegisterNewPrisoner()
    {
        activePrisoners++;
    }

    public Vector3 GetWaitingPoint()
    {
        return waitingAreaPoint.position;
    }

    public void TryEnterPrison(Npc npc)
    {
        if (npc == null) return;
        
        if (IsPrisonFull)
        {
            waitingNPCs.Add(npc);
            return;
        }

        EnterPrisonLogic(npc);
    }

    private void EnterPrisonLogic(Npc npc)
    {
        currentPrisonerCount++;
        AudioManager.Instance.Play(SoundType.PrisonerEnter);
        prisonerCountText.text = $"{currentPrisonerCount}/{maxCapacity}";
        NotifyStateChanged();

        if (waitingNPCs.Contains(npc)) waitingNPCs.Remove(npc);

        enteringCount++;
        if (prisonDoor != null) prisonDoor.SetActive(false);

        // [Restore]: 입구 이동 -> 180도 회전 -> 무작위 분산 이동(DOMove)
        npc.MoveTo(prisonEntrance.position, () => {
            npc.transform.DORotate(new Vector3(0, 180f, 0f), 0.5f).OnComplete(() => {
                
                // 감옥 내부 무작위 지점으로 살짝 흩어지는 연출
                float rangeX = 1.0f;
                float rangeZ = 0.5f;
                Vector3 randomPos = new Vector3(Random.Range(0, rangeX), 0f, Random.Range(0, rangeZ)) + npc.transform.position;
                
                npc.transform.DOMove(randomPos, 1.0f).OnComplete(() => {
                    npc.Rb.freezeRotation = true;
                });

                if (IsPrisonFull && !hasTriggeredExpandGuide)
                {
                    hasTriggeredExpandGuide = true;
                    if (TutorialManager.Instance != null)
                        TutorialManager.Instance.TriggerPrisonExpandGuide(prisonUnlockZone.transform, prisonUnlockTarget);
                }

                enteringCount--;
                if (enteringCount <= 0 && prisonDoor != null)
                {
                    prisonDoor.SetActive(true);
                }
            });
        });
    }

    private void NotifyStateChanged()
    {
        OnPrisonerCountChanged?.Invoke(currentPrisonerCount, maxCapacity);
    }
    
    public void RefreshWaitingQueue()
    {
        if (waitingNPCs.Count <= 0) return;

        // 현재 가용 용량 계산
        int availableSpace = maxCapacity - currentPrisonerCount;
        if (availableSpace <= 0) return;

        // 컬렉션 수정 방지를 위해 임시 리스트 사용
        List<Npc> toEnter = new List<Npc>();
        foreach (var npc in waitingNPCs)
        {
            if (npc == null) continue;
            toEnter.Add(npc);
            if (toEnter.Count >= availableSpace) break;
        }

        foreach (var npc in toEnter)
        {
            EnterPrisonLogic(npc);
        }
    }
}