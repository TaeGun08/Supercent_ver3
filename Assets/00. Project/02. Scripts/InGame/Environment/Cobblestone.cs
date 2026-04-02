using UnityEngine;

public class Cobblestone : MonoBehaviour, IPickupAble
{
    public Transform Transform => transform;

    public void Release()
    {
        // 풀매니저를 통한 반납 로직
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
