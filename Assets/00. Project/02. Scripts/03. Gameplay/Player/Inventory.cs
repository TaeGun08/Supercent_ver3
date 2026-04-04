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

        // [Tutorial Fix]: 각각의 스태커가 자신의 상태 변화를 독립적으로 통보해야 함
        stoneStacker.OnStackChanged += OnStoneChanged;
        potionStacker.OnStackChanged += OnPotionChanged;
        goldStacker.OnStackChanged += OnGoldChanged;
    }

    private void Start()
    {
        uIManager = UIManager.Instance;
    }

    private void OnStoneChanged()
    {
        UpdateGoldStackerPosition();
        if (stoneStacker.CurrentCount > 0) 
            TutorialManager.Instance.OnActionPerform(TutorialCondition.MineStone);
    }

    private void OnPotionChanged()
    {
        if (potionStacker.CurrentCount > 0) 
            TutorialManager.Instance.OnActionPerform(TutorialCondition.TakePotion);
    }

    private void OnGoldChanged()
    {
        if (goldStacker.CurrentCount > 0) 
            TutorialManager.Instance.OnActionPerform(TutorialCondition.GetGold);
    }

    private void UpdateGoldStackerPosition()
    {
        goldStacker.transform.localPosition =
            stoneStacker.CurrentCount > 0 ? goldOriginalLocalPos + goldOffsetWhenStoneExists : goldOriginalLocalPos;
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
        stackerMap.TryAdd(stacker.AcceptableType, stacker);
    }

    private void InitializeStackerMap()
    {
        RegisterStacker(stoneStacker);
        RegisterStacker(goldStacker);
        RegisterStacker(potionStacker);
    }

    public ItemStacker GetStacker(ItemType type) => stackerMap.GetValueOrDefault(type);

    private void OnDestroy()
    {
        if (stoneStacker != null) stoneStacker.OnStackChanged -= OnStoneChanged;
        if (potionStacker != null) potionStacker.OnStackChanged -= OnPotionChanged;
        if (goldStacker != null) goldStacker.OnStackChanged -= OnGoldChanged;
    }
    
    public ItemStacker StoneStacker => stoneStacker;
    public ItemStacker GoldStacker => goldStacker;
    public ItemStacker PotionStacker => potionStacker;
}
