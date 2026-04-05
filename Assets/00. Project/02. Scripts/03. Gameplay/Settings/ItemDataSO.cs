using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemDataSO : ScriptableObject
{
    [Header("Item Identification")]
    public ItemType itemType;
    public string itemName;

    [Header("Visual & Audio")]
    public float stackHeight = 0.5f;
    public SoundType pickupSound = SoundType.ItemPickup_Default;
    public SoundType dropSound = SoundType.ItemDrop_Default;
}
