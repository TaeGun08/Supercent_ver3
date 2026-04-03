using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemStacker : MonoBehaviour
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
    
    public void PushStack(IPickupAble pickupAble)
    {
        if (pickupAble == null) return;
        if (pickupAble.Type != acceptableType) return;
        if (IsFull) return;

        int targetIndex = stackedItems.Count;
        stackedItems.Push(pickupAble);
        OnCountChanged?.Invoke(stackedItems.Count);
        
        pickupAble.Transform.SetParent(transform);
        pickupAble.Transform.SetLocalPositionAndRotation(GetPositionAtIndex(targetIndex), Quaternion.identity);
    }
    
    public IPickupAble PopStack()
    {
        if (!HasItem) return null;
        
        IPickupAble pickupAble = stackedItems.Pop();
        OnCountChanged?.Invoke(stackedItems.Count);
        
        pickupAble.Transform.SetParent(null);
        
        return pickupAble;
    }
}