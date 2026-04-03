using UnityEngine;

public class PotionTable : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ItemStacker tableStacker;
    [SerializeField] private ItemStacker moneyStacker; 
    [SerializeField] private Gold goldPrefab; 
    [SerializeField] private PlayerDetectionZone playerDetectionZone;
    
    private bool isPlayerAtTable;
    private TransportWorker transportWorker;

    private void Awake()
    {
        playerDetectionZone.OnPlayerDetected += SetPlayerPresence;
        transportWorker = FindObjectOfType<TransportWorker>();

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
            // 플레이어가 테이블에 있거나, 운반 인부가 배급 위치에서 대기 중일 때 true
            bool isWorkerAtPoint = transportWorker != null && transportWorker.gameObject.activeInHierarchy && transportWorker.IsAtDistributionPoint();
            return (isPlayerAtTable || isWorkerAtPoint) && tableStacker.HasItem;
        }
    }
}
