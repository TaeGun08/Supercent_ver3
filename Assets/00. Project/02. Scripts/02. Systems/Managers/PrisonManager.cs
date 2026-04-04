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
        
        // [Tutorial Hook]: 감옥 만석 시 안내
        if (IsPrisonFull && !hasTriggeredExpandGuide)
        {
            hasTriggeredExpandGuide = true;
            TutorialManager.Instance.TriggerPrisonExpandGuide(prisonUnlockZone.transform, prisonUnlockTarget);
        }
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

        npc.MoveTo(prisonEntrance.position, () => {
            // [Revert]: 불필요한 정렬 연출(Rotate, RandomMove) 삭제
            if (npc.Rb != null) npc.Rb.freezeRotation = true;
            
            enteringCount--;
            if (enteringCount <= 0 && prisonDoor != null)
            {
                prisonDoor.SetActive(true);
            }
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