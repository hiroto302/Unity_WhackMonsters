using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// このクラスのインスタンスは一つであることを保証しなければならない
/// </summary>
/// <typeparam name="T">自身のクラス</typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    public static T Instance => instance;
    // {
    //     get {return instance;}
    // }

    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            // Destroy(this.gameObject);
            Debug.LogError("[Singleton] Trying to instantiate a second instance of a singleton class");
        }
        else
        {
            instance = (T) this;
        }
    }

    protected virtual void OnDestroy()
    {
        if ( instance == this)
        {
            instance = null;
        }
    }
}

