using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private UIManager uIManager;
    
    [Header("Stackers")]
    [SerializeField] private ItemStacker stoneStacker;
    [SerializeField] private ItemStacker goldStacker;
    [SerializeField] private ItemStacker potionStacker;

    private readonly Dictionary<ItemType, ItemStacker> stackerMap = new();

    [Header("Gold Offset Settings")]
    [SerializeField] private Vector3 goldOffsetWhenStoneExists = new(0, 0, -0.5f);
    private Vector3 goldOriginalLocalPos;

    private void Awake()
    {
        InitializeStackerMap();
        
        goldOriginalLocalPos = goldStacker.transform.localPosition;
        // [Sync]: 변경된 이벤트 명칭(OnStackChanged) 적용
        stoneStacker.OnStackChanged += UpdateGoldStackerPosition;
    }

    private void Start()
    {
        uIManager = UIManager.Instance;
    }

    private void InitializeStackerMap()
    {
        RegisterStacker(stoneStacker);
        RegisterStacker(goldStacker);
        RegisterStacker(potionStacker);
    }

    private void LateUpdate()
    {
        if (uIManager != null)
        {
            uIManager.UpdateGoldUI(goldStacker.CurrentCount);
        }
    }

    private void RegisterStacker(ItemStacker stacker)
    {
        if (stacker == null) return;
        
        ItemType type = stacker.AcceptableType;
        stackerMap.TryAdd(type, stacker);
    }

    public ItemStacker GetStacker(ItemType type)
    {
        return stackerMap.GetValueOrDefault(type);
    }

    private void UpdateGoldStackerPosition()
    {
        // [Sync]: 인자 없는 이벤트 방식으로 로직 변경
        goldStacker.transform.localPosition =
            stoneStacker.CurrentCount > 0 ? goldOriginalLocalPos + goldOffsetWhenStoneExists : goldOriginalLocalPos;
    }

    private void OnDestroy()
    {
        if (stoneStacker == null) return;
        stoneStacker.OnStackChanged -= UpdateGoldStackerPosition;
    }
    
    public ItemStacker StoneStacker => stoneStacker;
    public ItemStacker GoldStacker => goldStacker;
    public ItemStacker PotionStacker => potionStacker;
}
