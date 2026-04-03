using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonBase<PoolManager>
{
    private Dictionary<string, object> poolDict = new();
    
    public void CreatePool<T>(T prefab, string key = null, int initialCount = 10) where T : MonoBehaviour
    {
        string poolKey = key ?? typeof(T).Name;

        if (poolDict.ContainsKey(poolKey)) return;
        
        GameObject poolParent = new GameObject($"{poolKey}_Pool");
        poolParent.transform.SetParent(this.transform);

        ObjectPool<T> newPool = new ObjectPool<T>(prefab, initialCount, poolParent.transform);
        poolDict.Add(poolKey, newPool);
    }
    
    public T Get<T>(string key = null) where T : MonoBehaviour
    {
        string poolKey = key ?? typeof(T).Name;
        
        if (poolDict.TryGetValue(poolKey, out object poolObj))
        {
            return ((ObjectPool<T>)poolObj).Get();
        }
        
        Debug.LogError($"[PoolManager] ν  : {poolKey}");
        return null;
    }
    
    public void Return<T>(T obj, string key = null) where T : MonoBehaviour
    {
        string poolKey = key ?? typeof(T).Name;
        
        if (poolDict.TryGetValue(poolKey, out object poolObj))
        {
            ((ObjectPool<T>)poolObj).Return(obj);
        }
        else
        {
            Destroy(obj.gameObject);
        }
    }
}