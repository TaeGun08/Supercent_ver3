using UnityEngine;

public class Potion : MonoBehaviour, IPickupAble
{
    public Transform Transform => transform;
    public ItemType Type => ItemType.Potion;

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
