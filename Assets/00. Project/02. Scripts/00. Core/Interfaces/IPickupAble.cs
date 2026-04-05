using UnityEngine;

public interface IPickupAble
{
    public Transform Transform { get; }
    public ItemDataSO Data { get; }
    
    void Release();
}
