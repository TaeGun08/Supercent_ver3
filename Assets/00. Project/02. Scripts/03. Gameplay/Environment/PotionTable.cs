using UnityEngine;

public class PotionTable : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ItemStacker tableStacker;
    [SerializeField] private ItemStacker moneyStacker; 
    [SerializeField] private Gold goldPrefab; 
    [SerializeField] private PlayerDetectionZone playerDetectionZone;
    
    private bool isPlayerAtTable;
    private TransportWorker cachedWorker;

    private void Awake()
    {
        playerDetectionZone.OnPlayerDetected += SetPlayerPresence;
        PoolManager.Instance.CreatePool(goldPrefab, initialCount: 10);
        
        // [Tutorial Fix]: 테이블에 아이템이 들어올 때가 아니라, 골드가 생성될 때 단계를 넘기기 위해 기존 OnStackChanged 훅 제거
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

        // [Tutorial Hook]: 실제 골드가 맵에 생성되는 시점에 배급 완료(DistributePotion) 통보
        TutorialManager.Instance.OnActionPerform(TutorialCondition.DistributePotion);

        for (int i = 0; i < 10; i++)
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
            if (cachedWorker == null || !cachedWorker.gameObject.activeInHierarchy)
            {
                cachedWorker = FindObjectOfType<TransportWorker>();
            }

            bool isWorkerAtPoint = cachedWorker != null && cachedWorker.IsAtDistributionPoint();
            return (isPlayerAtTable || isWorkerAtPoint) && tableStacker.HasItem;
        }
    }
}
