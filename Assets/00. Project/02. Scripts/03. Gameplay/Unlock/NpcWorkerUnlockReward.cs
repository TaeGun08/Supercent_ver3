using UnityEngine;
using Project.Core.Interfaces;
using System.Collections.Generic;

public class NpcWorkerUnlockReward : MonoBehaviour, IUnlockReward
{
    [Header("Reward Settings")]
    [SerializeField] private List<GameObject> workersToActivate;

    private void Awake()
    {
        // 시작 시 모든 대상 워커를 비활성화 상태로 둡니다.
        if (workersToActivate != null)
        {
            foreach (var worker in workersToActivate)
            {
                if (worker != null) worker.SetActive(false);
            }
        }
    }

    public void Execute()
    {
        if (workersToActivate != null)
        {
            foreach (var worker in workersToActivate)
            {
                if (worker != null)
                {
                    worker.SetActive(true);
                    Debug.Log($"[Unlock] 인부 활성화 완료: {worker.name}");
                }
            }
        }
    }
}
