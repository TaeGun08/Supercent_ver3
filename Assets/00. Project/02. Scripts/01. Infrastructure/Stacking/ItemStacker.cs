using System;
using System.Collections.Generic;
using UnityEngine;

public interface IStackHolder
{
    bool IsFull { get; }
    bool HasItem { get; }
    int CurrentCount { get; }
    Vector3 GetNextLocalPosition();
    IPickupAble PopStack();
    void PushStack(IPickupAble item);
}

public class ItemStacker : MonoBehaviour, IStackHolder
{
    [Header("Stacker Settings")]
    [SerializeField] private ItemDataSO itemData;
    [SerializeField] private int maxStackCount = 10;
    
    [Header("Grid Settings")]
    [SerializeField] private int gridColumns = 1;
    [SerializeField] private int gridRows = 1;
    [SerializeField] private float spacingX = 0.5f;
    [SerializeField] private float spacingZ = 0.5f;
    
    private Stack<IPickupAble> stackedItems = new();
    private int pendingCount = 0; // 비행 중인 아이템 예약 카운트
    
    public bool IsFull => stackedItems.Count >= maxStackCount;
    public bool IsFullWithPending => (stackedItems.Count + pendingCount) >= maxStackCount;
    public bool HasItem => stackedItems.Count > 0;
    public int CurrentCount => stackedItems.Count;
    
    // [Data-Driven]: 메타데이터 접근
    public ItemDataSO ItemData => itemData;
    public ItemType AcceptableType => itemData != null ? itemData.itemType : ItemType.None;
    public float ItemHeight => itemData != null ? itemData.stackHeight : 0.5f;
    
    public event Action OnStackChanged;

    public Vector3 GetNextLocalPosition() => GetPositionAtIndex(stackedItems.Count + pendingCount);

    public Vector3 GetPositionAtIndex(int index)
    {
        int layerSize = gridColumns * gridRows;
        if (layerSize <= 1) return new Vector3(0, index * ItemHeight, 0);

        int layer = index / layerSize;
        int indexInLayer = index % layerSize;
        int row = indexInLayer / gridColumns;
        int col = indexInLayer % gridColumns;

        float xPos = (col - (gridColumns - 1) * 0.5f) * spacingX;
        float zPos = (row - (gridRows - 1) * 0.5f) * spacingZ;
        return new Vector3(xPos, layer * ItemHeight, zPos);
    }

    public void ReserveSlot() => pendingCount++;
    public void CancelReservation() => pendingCount = Mathf.Max(0, pendingCount - 1);
    
    public void PushStack(IPickupAble pickupAble)
    {
        pendingCount = Mathf.Max(0, pendingCount - 1);

        // [Fix]: 객체 참조 비교는 에셋 할당 실수에 취약하므로 Enum 값으로 검증
        if (pickupAble == null || pickupAble.Data == null || pickupAble.Data.itemType != AcceptableType) 
        {
            Debug.LogWarning($"[ItemStacker] 잘못된 아이템 투입 시도: {pickupAble?.Data?.itemName ?? "Unknown"} (Expected: {AcceptableType})");
            return;
        }

        if (IsFull)
        {
            pickupAble.Release();
            return;
        }

        stackedItems.Push(pickupAble);
        pickupAble.Transform.SetParent(transform);
        pickupAble.Transform.SetLocalPositionAndRotation(GetPositionAtIndex(stackedItems.Count - 1), Quaternion.identity);
        
        OnStackChanged?.Invoke();
    }
    
    public IPickupAble PopStack()
    {
        if (!HasItem) return null;
        
        IPickupAble pickupAble = stackedItems.Pop();
        pickupAble.Transform.SetParent(null);
        
        OnStackChanged?.Invoke();
        return pickupAble;
    }

    public void LimitMaxStackCount(int add) => maxStackCount += add;
}
