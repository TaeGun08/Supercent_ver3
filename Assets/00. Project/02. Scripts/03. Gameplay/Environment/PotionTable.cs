using UnityEngine;

public class PotionTable : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ItemStacker tableStacker;
    [SerializeField] private ItemStacker moneyStacker; 
    [SerializeField] private Gold goldPrefab; 
    [SerializeField] private PlayerDetectionZone playerDetectionZone;
    
    private bool isPlayerAtTable;
    private TransportWorker cachedWorker; // 캐싱용

    private void Awake()
    {
        playerDetectionZone.OnPlayerDetected += SetPlayerPresence;
        PoolManager.Instance.CreatePool(goldPrefab, initialCount: 10);
    }

    private void OnDestroy()
    {
        if (playerDetectionZone != null)
            playerDetectionZone.OnPlayerDetected -= SetPlayerPresence;
    }
    
    public void SetPlayerPresence(bool isPresent)
    {
        isPlayerAtTable = isPresent;
    }
    
    public void ProduceGold()
    {
        if (moneyStacker == null || moneyStacker.IsFull) return;

        for (int i = 0; i < 6; i++)
        {
            Gold gold = PoolManager.Instance.Get<Gold>();
            if (gold != null)
            {
                moneyStacker.PushStack(gold);
            }
        }
    }

    public ItemStacker GetStacker() => tableStacker;
    
    public bool CanDistribute 
    {
        get
        {
            // 인부가 해금되어 활성화된 시점에 다시 찾을 수 있도록 동적 검색 포함
            if (cachedWorker == null || !cachedWorker.gameObject.activeInHierarchy)
            {
                cachedWorker = FindObjectOfType<TransportWorker>();
            }

            bool isWorkerAtPoint = cachedWorker != null && cachedWorker.IsAtDistributionPoint();
            return (isPlayerAtTable || isWorkerAtPoint) && tableStacker.HasItem;
        }
    }
}
