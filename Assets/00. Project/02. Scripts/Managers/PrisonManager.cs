using System.Collections.Generic;
using UnityEngine;

public class PrisonManager : SingletonBase<PrisonManager>
{
    [Header("Prison Capacity")]
    [SerializeField] private int maxCapacity = 20;
    [SerializeField] private Transform prisonEntrance;
    [SerializeField] private Transform waitingAreaPoint;

    private int currentPrisonerCount;
    private HashSet<Npc> waitingNPCs = new();

    public bool IsFull => currentPrisonerCount >= maxCapacity;
    
    public void TryEnterPrison(Npc npc)
    {
        if (IsFull)
        {
            JoinWaitingArea(npc);
            return;
        }

        EnterPrisonLogic(npc);
    }

    private void EnterPrisonLogic(Npc npc)
    {
        currentPrisonerCount++;
        waitingNPCs.Remove(npc);
        
        npc.MoveTo(prisonEntrance.position, () => {
            npc.gameObject.SetActive(false);
        });
    }

    private void JoinWaitingArea(Npc npc)
    {
        waitingNPCs.Add(npc);
        
        Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        npc.MoveTo(waitingAreaPoint.position + randomOffset);
    }

    public void OnPrisonEmptySlot()
    {
        if (waitingNPCs.Count <= 0 || IsFull) return;
        var enumerator = waitingNPCs.GetEnumerator();
        if (enumerator.MoveNext())
        {
            TryEnterPrison(enumerator.Current);
        }
    }
}