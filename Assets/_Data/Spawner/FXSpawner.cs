using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSpawner : Spawner
{
    [Header("FXSpawner")]
    private static FXSpawner _instance;
    public static FXSpawner Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("Singleton instance has not been created yet!");
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (_instance == null)
        {
            _instance = this;
            if (transform.parent == null) DontDestroyOnLoad(gameObject);
            return;
        }

        if (_instance != this)
        {
            Debug.LogWarning("Another instance of FXSpawner already exists!");
            Destroy(gameObject);
            return;
        }
    }
}
