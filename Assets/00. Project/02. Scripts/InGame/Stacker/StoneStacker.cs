using System;
using UnityEngine;

public class StoneStacker : ItemStacker
{
    [SerializeField] private Transform pickupGold;
    
    private Vector3 originLocalPos;
    private Vector3 offsetLocalPos;

    private void Start()
    {
        originLocalPos = pickupGold.localPosition;
        
        offsetLocalPos = originLocalPos + new Vector3(0, 0, -0.5f); 
    }

    public override void PushStack(IPickupAble pickupAble)
    {
        if (HasItem == false) pickupGold.localPosition = offsetLocalPos;
        
        base.PushStack(pickupAble);
    }

    public override IPickupAble PopStack()
    {
        if (HasItem == false) pickupGold.localPosition = originLocalPos; 
        
        return base.PopStack();
    }
}