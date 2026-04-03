using UnityEngine;

public class PotionTable : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ItemStacker tableStacker;
    [SerializeField] private ItemStacker moneyStacker; // 골드가 생성되어 쌓일 곳
    [SerializeField] private Gold goldPrefab; // 생성할 골드 프리팹
    [SerializeField] private PlayerDetectionZone playerDetectionZone;
    
    private bool isPlayerAtTable;

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
    
    public bool CanDistribute => isPlayerAtTable && tableStacker.HasItem;
}