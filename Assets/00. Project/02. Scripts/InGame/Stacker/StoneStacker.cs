using System;
using UnityEngine;

public class StoneStacker : ItemStacker
{
    [SerializeField] private Transform pickupGold;
    
    public override void PushStack(IPickupAble pickupAble)
    {
        base.PushStack(pickupAble);
    }

    public override IPickupAble PopStack(Transform targetTrs, Vector3 pos)
    {
        return base.PopStack(targetTrs, pos);
    }
}
