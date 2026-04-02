using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonBase<PoolManager>
{
    private Dictionary<Type, object> poolDict = new Dictionary<Type, object>();
    
    public void CreatePool<T>(T prefab, int initialCount = 10) where T : MonoBehaviour
    {
        Type type = typeof(T);

        if (poolDict.ContainsKey(type)) return;
        GameObject poolParent = new GameObject($"{type.Name}_Pool");
        poolParent.transform.SetParent(this.transform);

        ObjectPool<T> newPool = new ObjectPool<T>(prefab, initialCount, poolParent.transform);
            
        poolDict.Add(type, newPool);
    }
    
    public T Get<T>() where T : MonoBehaviour
    {
        Type type = typeof(T);
        
        return poolDict.TryGetValue(type, out object poolObj) ? ((ObjectPool<T>)poolObj).Get() : null;
    }
    
    public void Return<T>(T obj) where T : MonoBehaviour
    {
        Type type = typeof(T);
        
        if (poolDict.TryGetValue(type, out object poolObj))
        {
            ((ObjectPool<T>)poolObj).Return(obj);
        }
        else
        {
            Destroy(obj.gameObject);
        }
    }
}