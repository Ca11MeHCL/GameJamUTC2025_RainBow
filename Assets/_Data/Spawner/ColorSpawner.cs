using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpawner : Spawner
{
    private static ColorSpawner _instance;
    public static ColorSpawner Instance {
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
            Debug.LogWarning("Another instance of ColorSpawner already exists!");
            Destroy(gameObject);
            return;
        }
    }
}
