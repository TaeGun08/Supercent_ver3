using UnityEngine;

public class Gold : MonoBehaviour, IPickupAble
{
    [SerializeField] private ItemDataSO itemData;
    public Transform Transform => transform;
    public ItemDataSO Data => itemData;

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
