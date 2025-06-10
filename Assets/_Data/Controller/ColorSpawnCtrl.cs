using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpawnCtrl : ImpBehaviour
{
    [SerializeField] protected ColorSpawner colorSpawner;
    public ColorSpawner ColorSpawner { get => colorSpawner; }

    [SerializeField] protected ColorSpawnPointsCtrl spawnPoints;
    public ColorSpawnPointsCtrl SpawnPoints { get => spawnPoints; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadColorSpawner();
        this.LoadSpawnPoints();
    }

    protected virtual void LoadColorSpawner()
    {
        if (colorSpawner != null) return;
        this.colorSpawner = GetComponent<ColorSpawner>();
        Debug.Log(transform.name + ": LoadColorSpawner", gameObject);
    }

    protected virtual void LoadSpawnPoints()
    {
        if (this.spawnPoints != null) return;
        this.spawnPoints = Transform.FindObjectOfType<ColorSpawnPointsCtrl>();
        Debug.Log(transform.name + ": LoadSpawnPoints", gameObject);
    }
}
