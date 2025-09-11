using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T>  : MonoBehaviour where T : MonoBehaviour
{
    protected static bool isDontDestroy = false;

    static T _instance;
    static public T instance
    {
        get
        {
            if( _instance == null)
            {
                _instance = CreateEntity();
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (isDontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }else if( _instance == this)
        {
            Debug.LogWarning($"�ظ�����ʵ��{typeof(T).Name},�ƻ�����.");
            Destroy(gameObject);
            return;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance=null;
        }
    }

    private static T CreateEntity()
    {
        GameObject obj=new GameObject(typeof(T).Name);

        if (isDontDestroy)
        {
            DontDestroyOnLoad(obj);
        }

        return obj.AddComponent<T>();
    }
}
