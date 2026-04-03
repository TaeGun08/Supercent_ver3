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
    private int activePrisoners = 0; // 현재 월드에 존재하는 전체 죄수 수 (스폰된 인원 포함)
    private int enteringCount = 0; 
    private readonly HashSet<Npc> waitingNPCs = new();

    public bool IsFull => activePrisoners >= maxCapacity;

    /// <summary>
    /// 새로운 죄수가 스폰되었을 때 호출 (수용량 예약)
    /// </summary>
    public void RegisterNewPrisoner()
    {
        activePrisoners++;
    }

    public Vector3 GetWaitingPoint()
    {
        Debug.Assert(waitingAreaPoint != null, "[PrisonManager] waitingAreaPoint가 설정되지 않았습니다.");
        return waitingAreaPoint.position;
    }

    public void TryEnterPrison(Npc npc)
    {
        if (npc == null) return;

        // 실제 수감 로직은 이미 등록된 인원을 처리하는 것이므로 IsFull 체크가 아닌 
        // 물리적 공간(enteringCount) 등을 관리합니다.
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