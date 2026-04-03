using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemStacker : MonoBehaviour
{
    [Header("Stacker Settings")]
    [SerializeField] private ItemType acceptableType;
    [SerializeField] protected int maxStackCount = 10;
    [SerializeField] protected float itemHeight = 0.5f;
    public float ItemHeight => itemHeight;

    [Header("Grid Settings")]
    [SerializeField] private int gridColumns = 1; // 열 (X축)
    [SerializeField] private int gridRows = 1;    // 행 (Z축)
    [SerializeField] private float spacingX = 0.5f;
    [SerializeField] private float spacingZ = 0.5f;
    
    protected Stack<IPickupAble> stackedItems = new();
    
    public bool IsFull => stackedItems.Count >= maxStackCount;
    public bool HasItem => stackedItems.Count > 0;
    public int CurrentCount => stackedItems.Count;
    public ItemType AcceptableType => acceptableType;
    
    public Action<int> OnCountChanged;
    
    public Vector3 GetNextLocalPosition()
    {
        return GetPositionAtIndex(stackedItems.Count);
    }

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
        float yPos = layer * itemHeight;

        return new Vector3(xPos, yPos, zPos);
    }
    
    public virtual void PushStack(IPickupAble pickupAble)
    {
        if (pickupAble == null) return;
        if (pickupAble.Type != acceptableType) return;
        if (IsFull) return;

        // 적재 전 현재 인덱스 저장
        int targetIndex = stackedItems.Count;
        stackedItems.Push(pickupAble);
        OnCountChanged?.Invoke(stackedItems.Count);
        
        pickupAble.Transform.SetParent(transform);
        // 저장된 정확한 인덱스로 위치 고정 (스냅 버그 해결)
        pickupAble.Transform.SetLocalPositionAndRotation(GetPositionAtIndex(targetIndex), Quaternion.identity);
    }
    
    public virtual IPickupAble PopStack()
    {
        if (!HasItem) return null;
        
        IPickupAble pickupAble = stackedItems.Pop();
        OnCountChanged?.Invoke(stackedItems.Count);
        
        pickupAble.Transform.SetParent(null);
        
        return pickupAble;
    }
}