using System.Collections.Generic;
using UnityEngine;

public class PrisonManager : SingletonBase<PrisonManager>
{
    [Header("Prison Capacity")]
    [SerializeField] private int maxCapacity = 20;
    [SerializeField] private Transform prisonEntrance;
    [SerializeField] private Transform waitingAreaPoint;

    private int currentPrisonerCount;
    private readonly HashSet<Npc> waitingNPCs = new();

    public bool IsFull => currentPrisonerCount >= maxCapacity;

    /// <summary>
    /// 대기 구역 내의 랜덤한 목표 지점 반환
    /// </summary>
    public Vector3 GetRandomWaitingPoint()
    {
        Debug.Assert(waitingAreaPoint != null, "[PrisonManager] waitingAreaPoint가 설정되지 않았습니다.");
        Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        return waitingAreaPoint.position + randomOffset;
    }
    
    public void TryEnterPrison(Npc npc)
    {
        if (npc == null) return;

        // [Logic Change]: 자리가 없으면 리스트에 등록만 하고 대기 (이동은 Prisoner가 직접 수행함)
        if (IsFull)
        {
            waitingNPCs.Add(npc);
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