using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpawner : Spawner
{
    [Header("ColorSpawner")]
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
            //if (transform.parent == null) DontDestroyOnLoad(gameObject);
            return;
        }

        if (_instance != this)
        {
            Debug.LogWarning("Another instance of ColorSpawner already exists!");
            Destroy(gameObject);
            return;
        }
    }

    [SerializeField] protected int energySpawn = 10;
    public int EnergySpawn { get { return energySpawn; } set { energySpawn = value; } }

    [SerializeField] protected int energySpawnIncrease = 10;
    public int EnergySpawnIncrease { get { return energySpawnIncrease; } }

    public bool CanSpawn()
    {
        if (!EnergyManager.Instance.CanUseEnergy(energySpawn))
        {
            Debug.Log("Khong du nang luong");
            return false;
        }
        this.energySpawn += this.energySpawnIncrease;
        EnergyManager.Instance.EnergyNeedTxt.text = this.energySpawn.ToString();
        return true;
    }
}
