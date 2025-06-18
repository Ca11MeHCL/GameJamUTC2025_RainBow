using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellColorCtrl : ImpBehaviour
{
    [Header("CellColorCtrl")]
    [SerializeField] protected List<PetColorCtrl> petColorCtrls;
    public List<PetColorCtrl> PetColorCtrls { get => petColorCtrls; }

    [SerializeField] protected Vector3 colorDeviation = new Vector3(0, 0.19f, 0);
    public Vector3 GetColorDeviation { get => colorDeviation; }

    public virtual void LoadPetColorCtrl()
    {
        this.petColorCtrls.Clear();
        foreach (Transform _colorSpawned in transform)
        {
            PetColorCtrl _pet = _colorSpawned.GetComponent<PetColorCtrl>();
            this.petColorCtrls.Add(_pet);
        }
        if (this.petColorCtrls.Count > 1)
        {
            this.LoadColorsSpawnedLocation();
        }
    }

    protected virtual void LoadColorsSpawnedLocation()
    {
        for (int i = 0; i < this.petColorCtrls.Count; i++)
        {
            this.petColorCtrls[i].transform.position = this.transform.position + i * this.colorDeviation;
            this.petColorCtrls[i].SpriteRenderer.sortingOrder = i + 1;
        }
    }
}
