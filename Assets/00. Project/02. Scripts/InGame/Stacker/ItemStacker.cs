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
    
    protected Stack<IPickupAble> stackedItems = new();
    
    public bool IsFull => stackedItems.Count >= maxStackCount;
    public bool HasItem => stackedItems.Count > 0;
    public int CurrentCount => stackedItems.Count;
    public ItemType AcceptableType => acceptableType;
    
    public Action<int> OnCountChanged;
    
    public Vector3 GetNextLocalPosition()
    {
        return new Vector3(0, stackedItems.Count * itemHeight, 0);
    }
    
    public virtual void PushStack(IPickupAble pickupAble)
    {
        if (pickupAble == null) return;
        if (pickupAble.Type != acceptableType) return;
        if (IsFull) return;

        stackedItems.Push(pickupAble);
        OnCountChanged?.Invoke(stackedItems.Count);
        
        pickupAble.Transform.SetParent(transform);
        pickupAble.Transform.SetLocalPositionAndRotation(GetNextLocalPosition(), Quaternion.identity);
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