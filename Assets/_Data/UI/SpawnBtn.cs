using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBtn : UIButton
{
    [Header("SpawnBtn")]
    [SerializeField] protected ColorSpawnerRandom colorSpawnerRandom;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadColorSpawnerRandom();
    }

    protected virtual void LoadColorSpawnerRandom()
    {
        if (this.colorSpawnerRandom != null) return;
        this.colorSpawnerRandom = FindObjectOfType<ColorSpawnerRandom>();
        Debug.Log(transform.name + ": LoadColorSpawnerRandom", gameObject);
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        colorSpawnerRandom.ColorSpawnRandom();
    }
}
