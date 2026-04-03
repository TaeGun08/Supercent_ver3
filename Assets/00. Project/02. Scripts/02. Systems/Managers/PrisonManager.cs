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
    
    public void ExpandCapacity(int additionalAmount)
    {
        maxCapacity += additionalAmount;
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
        
        if (currentPrisonerCount >= maxCapacity)
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
            npc.transform.DORotate(new Vector3(0, npc.transform.eulerAngles.y + 180f, 0), 0.5f)
                .OnComplete(() => {
                    if (npc.Rb != null) npc.Rb.freezeRotation = true;

                    float randomPosX = Random.Range(-0.5f, 0.5f);
                    float randomPosZ = Random.Range(-1f, 1f);
                    Vector3 randomPos = new Vector3(randomPosX, 0f, randomPosZ) + npc.transform.position;
                    npc.transform.DOMove(randomPos, 1f);
                    
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