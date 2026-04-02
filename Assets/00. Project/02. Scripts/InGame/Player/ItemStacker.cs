using System.Collections.Generic;
using UnityEngine;

public class ItemStacker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxStackCount = 10;
    [SerializeField] private Transform targetTrs;
    [SerializeField] private float height = 1f;
    [SerializeField] private float duration = 0.5f;
    
    private Stack<IPickupAble> stackedItems = new();
    
    public bool IsFull => stackedItems.Count >= maxStackCount;
    
    public void PushStack(IPickupAble pickupAble)
    {
        if (IsFull || pickupAble == null) return;

        int count = stackedItems.Count;
        pickupAble?.Transform.SetParent(targetTrs);
        DOParabolicMove.MoveToDynamicTarget(pickupAble.Transform, targetTrs, height, duration, count * height);
        stackedItems.Push(pickupAble);
    }
}