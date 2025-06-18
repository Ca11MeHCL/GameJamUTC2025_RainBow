using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpawnPointsCtrl : SpawnPoints
{
    [Header("ColorSpawnPointsCtrl")]
    [SerializeField] protected List<CellColorCtrl> cellColorCtrls;
    public List<CellColorCtrl> CellColorCtrls { get => cellColorCtrls; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCellColorCtrl();
    }

    protected virtual void LoadCellColorCtrl()
    {
        if (this.cellColorCtrls.Count > 0) return;
        foreach (Transform _point in transform)
        {
            CellColorCtrl _cell = _point.GetComponent<CellColorCtrl>();
            this.cellColorCtrls.Add(_cell);
        }
        Debug.Log(transform.name + ": LoadCellColorCtrl", gameObject);
    }

    public void LoadColorSpawnedInList()
    {
        foreach (CellColorCtrl _cell in cellColorCtrls)
        {
            _cell.LoadPetColorCtrl();
        }
    }

    public void LoadActiveCellColorCtrls()
    {
        this.cellColorCtrls.Clear();
        foreach (Transform _point in transform)
        {
            if (!_point.gameObject.activeInHierarchy) continue;

            CellColorCtrl _cell = _point.GetComponent<CellColorCtrl>();
            if (_cell != null)
            {
                this.cellColorCtrls.Add(_cell);
            }
        }

        //Debug.Log(transform.name + ": LoadActiveCellColorCtrls", gameObject);
    }
}
