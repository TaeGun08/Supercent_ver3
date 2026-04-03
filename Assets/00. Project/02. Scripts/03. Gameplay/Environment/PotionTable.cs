using UnityEngine;

public class PotionTable : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ItemStacker tableStacker;
    [SerializeField] private PlayerDetectionZone playerDetectionZone;
    
    private bool isPlayerAtTable;

    private void Awake()
    {
        playerDetectionZone.OnPlayerDetected += SetPlayerPresence;
    }

    private void OnDestroy()
    {
        playerDetectionZone.OnPlayerDetected -= SetPlayerPresence;
    }
    
    public void SetPlayerPresence(bool isPresent)
    {
        isPlayerAtTable = isPresent;
    }

    public ItemStacker GetStacker() => tableStacker;
    
    public bool CanDistribute => isPlayerAtTable && tableStacker.HasItem;
}