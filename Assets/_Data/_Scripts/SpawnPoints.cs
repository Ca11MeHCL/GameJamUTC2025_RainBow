using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : ImpBehaviour
{
    [Header("SpawnPoints")]
    [SerializeField] protected List<Transform> points;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPoints();
    }

    protected virtual void LoadPoints()
    {
        if (this.points.Count > 0) return;
        foreach (Transform point in transform)
        {
            this.points.Add(point);
        }
        Debug.Log(transform.name + ": LoadPoints", gameObject);
    }
}
