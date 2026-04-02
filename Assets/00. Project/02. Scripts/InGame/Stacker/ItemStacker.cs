using System.Collections.Generic;
using UnityEngine;

public abstract class ItemStacker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] protected int maxStackCount = 10;
    [SerializeField] protected float height = 0.5f;
    [SerializeField] protected float duration = 0.1f;
    
    protected Stack<IPickupAble> stackedItems = new();
    
    public bool IsFull => stackedItems.Count >= maxStackCount;
    public bool HasItem => stackedItems.Count > 0;
    
    public virtual void PushStack(IPickupAble pickupAble)
    {
        if (IsFull || pickupAble == null) return;

        int count = stackedItems.Count;
        pickupAble?.Transform.SetParent(transform);
        DOParabolicMove.MoveToDynamicTarget(pickupAble.Transform, transform, height, duration, count * height);
        stackedItems.Push(pickupAble);
    }

    public virtual IPickupAble PopStack(Transform targetTrs, Vector3 pos)
    {
        IPickupAble pickupAble = stackedItems.Pop();
        pickupAble?.Transform.SetParent(targetTrs);
        DOParabolicMove.MoveToStaticPosition(pickupAble?.Transform, pos, height, duration);
        return pickupAble;
    }
}