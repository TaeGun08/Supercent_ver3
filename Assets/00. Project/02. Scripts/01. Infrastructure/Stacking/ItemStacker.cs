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
    
    public bool IsFull => stackedItems.Count >= maxStackCount;
    public bool HasItem => stackedItems.Count > 0;
    public int CurrentCount => stackedItems.Count;
    public ItemType AcceptableType => acceptableType;
    
    public event Action OnStackChanged; // 범용 이벤트로 통합

    public Vector3 GetNextLocalPosition() => GetPositionAtIndex(stackedItems.Count);

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
    
    public void PushStack(IPickupAble pickupAble)
    {
        if (pickupAble == null || pickupAble.Type != acceptableType || IsFull) return;

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
