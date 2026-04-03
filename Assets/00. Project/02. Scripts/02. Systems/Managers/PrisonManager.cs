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

    public bool IsFull => activePrisoners >= maxCapacity;
    
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
        
        if (currentPrisonerCount >= maxCapacity)
        {
            waitingNPCs.Add(npc);
            return;
        }

        EnterPrisonLogic(npc);
    }

    private void EnterPrisonLogic(Npc npc)
    {
        // 이미 activePrisoners에 포함되어 있으므로 카운트만 수감 완료로 전이
        currentPrisonerCount++;
        waitingNPCs.Remove(npc);

        enteringCount++;
        if (prisonDoor != null) prisonDoor.SetActive(false);

        npc.MoveTo(prisonEntrance.position, () => {
            npc.transform.DORotate(new Vector3(0, npc.transform.eulerAngles.y + 180f, 0), 0.5f)
                .OnComplete(() => {
                    if (npc.Rb != null) npc.Rb.freezeRotation = true;

                    enteringCount--;
                    if (enteringCount <= 0 && prisonDoor != null)
                    {
                        prisonDoor.SetActive(true);
                    }
                });
        });
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