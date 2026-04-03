using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Stackers")]
    [SerializeField] private ItemStacker stoneStacker;
    [SerializeField] private ItemStacker goldStacker;
    [SerializeField] private ItemStacker potionStacker;

    private readonly Dictionary<ItemType, ItemStacker> stackerMap = new();

    [Header("Gold Offset Settings")]
    [SerializeField] private Vector3 goldOffsetWhenStoneExists = new Vector3(0, 0, -0.5f);
    private Vector3 goldOriginalLocalPos;

    private void Awake()
    {
        InitializeStackerMap();
        
        goldOriginalLocalPos = goldStacker.transform.localPosition;
        stoneStacker.OnCountChanged += UpdateGoldStackerPosition;
    }

    private void InitializeStackerMap()
    {
        // 명시적 등록 (Fail-Fast 포함)
        RegisterStacker(stoneStacker);
        RegisterStacker(goldStacker);
        RegisterStacker(potionStacker);
    }

    private void RegisterStacker(ItemStacker stacker)
    {
        if (stacker == null) return;
        
        ItemType type = stacker.AcceptableType;
        if (stackerMap.ContainsKey(type))
        {
            Debug.LogWarning($"[Inventory] 중복된 타입의 스태커가 등록되었습니다: {type}");
            return;
        }
        stackerMap.Add(type, stacker);
    }

    public ItemStacker GetStacker(ItemType type)
    {
        return stackerMap.TryGetValue(type, out var stacker) ? stacker : null;
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