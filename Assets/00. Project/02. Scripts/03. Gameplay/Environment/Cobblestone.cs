using UnityEngine;

public class Cobblestone : MonoBehaviour, IPickupAble
{
    public Transform Transform => transform;
    public ItemType Type => ItemType.Stone;

    public void Release()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.Return(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
