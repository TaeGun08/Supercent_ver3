using UnityEngine;

public class Potion : MonoBehaviour, IPickupAble
{
    public Transform Transform => transform;

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
