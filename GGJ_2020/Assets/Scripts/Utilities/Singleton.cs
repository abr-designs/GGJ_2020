using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    public static T Instance { get; private set; }
    private static T _instance;

    protected void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError($"WARNING YOU ARE TRYING TO CREATE NEW INSTANCE OF {typeof(T)} WHEN ONE ALREADY EXISTS");
            Destroy(gameObject);
        }

        _instance = this as T;
    }
}
