using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawnerCtrl : ImpBehaviour
{
    [Header("ColorSpawnCtrl")]
    [SerializeField] protected ColorBulletSpawner colorBulletSpawner;
    public ColorBulletSpawner ColorBulletSpawner { get => colorBulletSpawner; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadColorBulletSpawner();
    }

    protected virtual void LoadColorBulletSpawner()
    {
        if (colorBulletSpawner != null) return;
        this.colorBulletSpawner = GetComponent<ColorBulletSpawner>();
        Debug.Log(transform.name + ": LoadColorBulletSpawner", gameObject);
    }
}
