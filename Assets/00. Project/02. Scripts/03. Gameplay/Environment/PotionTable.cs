using UnityEngine;

public class PotionTable : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ItemStacker tableStacker;
    
    private bool isPlayerAtTable;
    
    public void SetPlayerPresence(bool isPresent)
    {
        isPlayerAtTable = isPresent;
    }

    public ItemStacker GetStacker() => tableStacker;
    
    public bool CanDistribute => isPlayerAtTable && tableStacker.HasItem;
}