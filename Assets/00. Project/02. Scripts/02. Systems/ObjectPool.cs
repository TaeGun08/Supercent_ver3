using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;

    public ObjectPool(T prefab, int initialCount, Transform parent = null)
    {
        Debug.Assert(prefab != null, $"[ObjectPool] {typeof(T).Name} 프리팹이 null입니다. 풀을 생성할 수 없습니다.");
        if (prefab == null) return;

        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialCount; i++)
        {
            var obj = CreatePool();
            if (obj != null)
            {
                obj.gameObject.SetActive(false);
                pool.Enqueue(obj);
            }
        }
    }

    private T CreatePool()
    {
        if (prefab == null) return null;
        var obj = Object.Instantiate(prefab, parent);
        return obj;
    }

    public T Get()
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;  
        }

        obj = CreatePool();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}