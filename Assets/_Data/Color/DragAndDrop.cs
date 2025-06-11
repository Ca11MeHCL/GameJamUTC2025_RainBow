using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DragAndDrop : ImpBehaviour
{
    [Header("DragAndDrop")]
    [SerializeField] protected CellColorCtrl cellColorCtrl;
    public CellColorCtrl CellColorCtrl { get => cellColorCtrl; }

    [SerializeField] protected List<PetColorCtrl> draggedPets;
    [SerializeField] protected List<Vector3> originalPositions;

    bool _dragging = false;
    DragAndDrop _targetDrop;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCellColorCtrl();
    }

    protected virtual void LoadCellColorCtrl()
    {
        if (cellColorCtrl != null) return;
        this.cellColorCtrl = GetComponent<CellColorCtrl>();
        Debug.Log(transform.name + ": LoadCellColorCtrl", gameObject);
    }

    private void OnMouseDown()
    {
        this._dragging = true;

        foreach (PetColorCtrl p in this.cellColorCtrl.PetColorCtrls)
        {
            p.SpriteRenderer.sortingOrder += 10;
            this.draggedPets.Add(p);
            this.originalPositions.Add(p.transform.position);
        }
    }

    private void OnMouseDrag()
    {
        if (!this._dragging) return;
        this.UpdateDraggedTransformLocation();
    }

    private void OnMouseUp()
    {
        this._dragging = false;
        this.CheckDropLocation();

        this.draggedPets.Clear();
        this.originalPositions.Clear();
    }

    protected virtual void UpdateDraggedTransformLocation()
    {
        for (int i = 0; i < this.draggedPets.Count; i++)
        {
            this.draggedPets[i].transform.position = this.GetMousePos2D() + i * this.cellColorCtrl.GetColorDeviation;
        }
    }

    protected virtual Vector3 GetMousePos2D()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePos.x, mousePos.y, 0f);
    }

    protected virtual void CheckDropLocation()
    {
        Vector3 _dropPoint = GetMousePos2D();
        Collider2D _hitCollider = Physics2D.OverlapPoint(_dropPoint, LayerMask.GetMask("DragAndDrop"));

        if (_hitCollider != null && _hitCollider.gameObject != this.gameObject)
        {
            _targetDrop = _hitCollider.GetComponent<DragAndDrop>();
            if (_targetDrop != null)
            {
                if (MergePetCondition())
                {
                    for (int i = 0; i < this.draggedPets.Count; i++)
                    {
                        this.draggedPets[i].transform.SetParent(_targetDrop.transform);
                        this.draggedPets[i].transform.position = _targetDrop.transform.position + i * this.cellColorCtrl.GetColorDeviation;
                    }
                    this.cellColorCtrl.LoadPetColorCtrl();
                    _targetDrop.cellColorCtrl.LoadPetColorCtrl();
                    return;

                }
            }
        }

        for (int i = 0; i < draggedPets.Count; i++)
        {
            draggedPets[i].transform.position = originalPositions[i];
        }

        Debug.Log("Khong ghep duoc");
    }

    protected virtual bool MergePetCondition()
    {
        if (_targetDrop.CellColorCtrl.PetColorCtrls.Count == 0) return true;
        if (draggedPets.Count != _targetDrop.CellColorCtrl.PetColorCtrls.Count) return false;

        List<PetColorCtrl> onlyInA = draggedPets.Where(p => !_targetDrop.CellColorCtrl.PetColorCtrls
            .Any(q => q.gameObject.name == p.gameObject.name)).ToList();

        List<PetColorCtrl> onlyInB = _targetDrop.CellColorCtrl.PetColorCtrls.Where(p => !draggedPets
            .Any(q => q.gameObject.name == p.gameObject.name)).ToList();

        List<PetColorCtrl> diff = onlyInA.Concat(onlyInB).ToList();

        if (diff.Count > 2 || diff.Count == 0) return false;

        for (int i = draggedPets.Count - 1; i >= 0; i--)
        {
            PetColorCtrl p = draggedPets[i];
            if (p.gameObject.name != onlyInA[0].gameObject.name)
            {
                ColorSpawner.Instance.Despawn(p.transform);
                draggedPets.RemoveAt(i);
            }
        }

        return true;
    }
}
