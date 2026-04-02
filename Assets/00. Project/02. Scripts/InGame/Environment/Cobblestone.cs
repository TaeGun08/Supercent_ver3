using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobblestone : MonoBehaviour, IPickupAble
{
    public Transform Transform => transform;

    public void OnPickup(Transform holderHand)
    {
        //DOParabolicMove.MoveToDynamicTarget()
    }

    public void OnPutDown(Vector3 dropPosition)
    {
        throw new System.NotImplementedException();
    }
}
