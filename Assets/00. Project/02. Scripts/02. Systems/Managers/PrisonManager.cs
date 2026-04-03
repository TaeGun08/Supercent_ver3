using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PrisonManager : SingletonBase<PrisonManager>
{
    [Header("Prison Capacity")]
    [SerializeField] private int maxCapacity = 20;
    [SerializeField] private Transform prisonEntrance;
    [SerializeField] private Transform waitingAreaPoint;
    [SerializeField] private GameObject prisonDoor; // 감옥 문 오브젝트

    private int currentPrisonerCount;
    private int enteringCount = 0; // 현재 입구로 이동 중인 인원 수
    private readonly HashSet<Npc> waitingNPCs = new();

    public bool IsFull => currentPrisonerCount >= maxCapacity;
    
    public Vector3 GetRandomWaitingPoint()
    {
        Debug.Assert(waitingAreaPoint != null, "[PrisonManager] waitingAreaPoint가 설정되지 않았습니다.");
        Vector3 randomOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        return waitingAreaPoint.position + randomOffset;
    }
    
    public void TryEnterPrison(Npc npc)
    {
        if (npc == null) return;

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
        
        // [Door Control]: 입소 시작 시 문 열기 (비활성화)
        enteringCount++;
        if (prisonDoor != null) prisonDoor.SetActive(false);

        npc.MoveTo(prisonEntrance.position, () => {
            npc.transform.DORotate(new Vector3(0, npc.transform.eulerAngles.y + 180f, 0), 0.5f)
                .OnComplete(() => {
                    if (npc.Rb != null) npc.Rb.freezeRotation = true;

                    // [Door Control]: 정렬 완료 후 문 닫기 체크
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