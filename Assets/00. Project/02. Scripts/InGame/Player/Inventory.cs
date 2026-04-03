using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Stackers")]
    [SerializeField] private ItemStacker stoneStacker;
    [SerializeField] private ItemStacker goldStacker;
    [SerializeField] private ItemStacker potionStacker;

    [Header("Gold Offset Settings")]
    [SerializeField] private Vector3 goldOffsetWhenStoneExists = new Vector3(0, 0, -0.5f);
    private Vector3 goldOriginalLocalPos;

    private void Awake()
    {
        goldOriginalLocalPos = goldStacker.transform.localPosition;
        
        stoneStacker.OnCountChanged += UpdateGoldStackerPosition;
    }

    private void UpdateGoldStackerPosition(int stoneCount)
    {
        goldStacker.transform.localPosition =
            stoneCount > 0 ? goldOriginalLocalPos + goldOffsetWhenStoneExists : goldOriginalLocalPos;
    }

    private void OnDestroy()
    {
        if (stoneStacker == null) return;
        stoneStacker.OnCountChanged -= UpdateGoldStackerPosition;
    }
    
    public ItemStacker StoneStacker => stoneStacker;
    public ItemStacker GoldStacker => goldStacker;
    public ItemStacker PotionStacker => potionStacker;
}