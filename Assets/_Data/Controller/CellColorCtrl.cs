using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellColorCtrl : ImpBehaviour
{
    [SerializeField] protected List<Transform> colorsSpawned;
    public List<Transform> ColorsSpawned { get => colorsSpawned; }

    public virtual void LoadColorsSpawned()
    {
        colorsSpawned.Clear();
        foreach (Transform _colorSpawned in transform)
        {
            this.colorsSpawned.Add(_colorSpawned);
        }
    }
}
