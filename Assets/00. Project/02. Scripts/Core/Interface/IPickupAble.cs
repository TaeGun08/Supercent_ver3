using UnityEngine;

public interface IPickupAble
{
    public Transform Transform { get; }
    
    public void OnPickup(Transform holderHand);

    public void OnPutDown(Vector3 dropPosition);
}
