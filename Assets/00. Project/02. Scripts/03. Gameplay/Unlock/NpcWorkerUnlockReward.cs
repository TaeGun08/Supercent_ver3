using UnityEngine;
using Project.Core.Interfaces;

public class NpcWorkerUnlockReward : MonoBehaviour, IUnlockReward
{
    [Header("Reward Settings")]
    [SerializeField] private GameObject workerSpawner;

    private void Awake()
    {
        // 시작 시 스포너를 비활성화 상태로 둡니다 (해금 전까지)
        if (workerSpawner != null)
        {
            workerSpawner.SetActive(false);
        }
    }

    public void Execute()
    {
        if (workerSpawner != null)
        {
            workerSpawner.SetActive(true);
            Debug.Log($"[Unlock] 새로운 인부 스포너가 활성화되었습니다: {workerSpawner.name}");
        }
    }
}
