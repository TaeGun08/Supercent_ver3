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
    [SerializeField] private ItemType acceptableType;
    [SerializeField] private int maxStackCount = 10;
    [SerializeField] private float itemHeight = 0.5f;
    public float ItemHeight => itemHeight;

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
    public ItemType AcceptableType => acceptableType;
    
    public event Action OnStackChanged; // 범용 이벤트로 통합

    public Vector3 GetNextLocalPosition() => GetPositionAtIndex(stackedItems.Count + pendingCount);

    public Vector3 GetPositionAtIndex(int index)
    {
        int layerSize = gridColumns * gridRows;
        if (layerSize <= 1) return new Vector3(0, index * itemHeight, 0);

        int layer = index / layerSize;
        int indexInLayer = index % layerSize;
        int row = indexInLayer / gridColumns;
        int col = indexInLayer % gridColumns;

        float xPos = (col - (gridColumns - 1) * 0.5f) * spacingX;
        float zPos = (row - (gridRows - 1) * 0.5f) * spacingZ;
        return new Vector3(xPos, layer * itemHeight, zPos);
    }

    public void ReserveSlot() => pendingCount++;
    public void CancelReservation() => pendingCount = Mathf.Max(0, pendingCount - 1);
    
    public void PushStack(IPickupAble pickupAble)
    {
        pendingCount = Mathf.Max(0, pendingCount - 1);

        if (pickupAble == null || pickupAble.Type != acceptableType) return;

        if (IsFull)
        {
            // [Fail-Fast] 가득 찬 상태에서 적재 시도 시 공중에 멈추지 않고 즉시 풀 반납
            pickupAble.Release();
            Debug.LogWarning($"[ItemStacker] {gameObject.name} is full. Item released to pool.");
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
