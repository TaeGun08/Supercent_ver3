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
        UpdateUI();
        Debug.Log($"[Prison] 수용량이 확장되었습니다. 현재 최대 수용량: {maxCapacity}");
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
        UpdateUI();
        waitingNPCs.Remove(npc);

        enteringCount++;
        if (prisonDoor != null) prisonDoor.SetActive(false);

        // [Restore]: 입구 이동 -> 180도 회전 -> 무작위 분산 이동(DOMove)
        npc.MoveTo(prisonEntrance.position, () => {
            npc.transform.DORotate(new Vector3(0, 180f, 0f), 0.5f).OnComplete(() => {
                
                // 감옥 내부 무작위 지점으로 살짝 흩어지는 연출
                float rangeX = 1.0f;
                float rangeZ = 1.0f;
                Vector3 randomPos = new Vector3(Random.Range(-rangeX, rangeX), 0f, Random.Range(-rangeZ, rangeZ)) + npc.transform.position;
                
                npc.transform.DOMove(randomPos, 1.0f).OnComplete(() => {
                    if (npc.Rb != null) npc.Rb.isKinematic = true; // 최종 도착 시 물리 시뮬레이션 중단
                    if (npc.Col != null) npc.Col.enabled = false; // 콜라이더 비활성화
                });

                // [Tutorial Hook]: 실제 수감 인원이 만석이 되었을 때 안내 트리거
                if (IsPrisonFull && !hasTriggeredExpandGuide)
                {
                    hasTriggeredExpandGuide = true;
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

    private void UpdateUI()
    {
        if (prisonerCountText != null)
        {
            prisonerCountText.text = $"{currentPrisonerCount} / {maxCapacity}";
        }
    }
    
    public void RefreshWaitingQueue()
    {
        if (waitingNPCs.Count <= 0 || IsFull) return;

        waitingNPCs.RemoveWhere(npc => npc == null);

        var enumerator = waitingNPCs.GetEnumerator();
        if (enumerator.MoveNext())
        {
            TryEnterPrison(enumerator.Current);
        }
    }
}