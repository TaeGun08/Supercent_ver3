using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    private static readonly object lockObj = new();
    private static bool applicationIsQuitting;
    private static bool isInitialized;

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton] {typeof(T)} 접근 시도됨: 애플리케이션 종료 중");
                return null;
            }

            lock (lockObj)
            {
                if (instance != null) return instance;
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    var singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                    DontDestroyOnLoad(singletonObject);
                }

                isInitialized = true;

                return instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);

            isInitialized = true;
        }
        else if (instance != this)
        {
            Debug.LogWarning($"중복된 인스터스가 존재하여 파괴하였습니다.");
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (instance != this) return;
        instance = null;
        isInitialized = false;
        applicationIsQuitting = true;
    }
}
