using UnityEngine;

public interface IMiningTarget
{
    public Transform Transform { get; }

    public IPickupAble MineResource(bool isFull);
}