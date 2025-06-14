using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpawnerRandom : ImpBehaviour
{
    [Header("ColorSpawnerRandom")]
    [SerializeField] protected ColorSpawnCtrl colorSpawnCtrl;
    public ColorSpawnCtrl ColorSpawnCtrl { get => colorSpawnCtrl; }

    List<int> _checkRandoms;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadColorSpawnCtrl();
    }

    protected virtual void LoadColorSpawnCtrl()
    {
        if (this.colorSpawnCtrl != null) return;
        this.colorSpawnCtrl = GetComponent<ColorSpawnCtrl>();
        Debug.Log(transform.name + ": LoadColorSpawnCtrl", gameObject);
    }

    public virtual void ColorSpawnRandom()
    {
        this.LoadChecckRandom();
        if (this.RandomPoint() == null)
        {
            Debug.Log("Khong con cho de spawn");
            return;
        }
        if (!this.colorSpawnCtrl.ColorSpawner.CanSpawn()) return;

        Transform _randomPoint = this.RandomPoint().transform;
        Vector3 pos = _randomPoint.position;
        Quaternion rot = transform.rotation;

        Transform prefab = this.colorSpawnCtrl.ColorSpawner.RandomPrefab();
        Transform obj = this.colorSpawnCtrl.ColorSpawner.Spawn(prefab, pos, rot);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.SpawnColorSound);
        obj.gameObject.SetActive(true);
        if (obj.gameObject.GetComponent<PetColorCtrl>().SpriteRenderer.sortingOrder > 10)
            obj.gameObject.GetComponent<PetColorCtrl>().SpriteRenderer.sortingOrder -= 10;
        obj.SetParent(_randomPoint.transform);

        this.colorSpawnCtrl.SpawnPoints.LoadColorSpawnedInList();
    }

    protected virtual CellColorCtrl RandomPoint()
    {
        if (this._checkRandoms.Count == 0) return null;
        int _rand = Random.Range(0, this._checkRandoms.Count);
        return this.colorSpawnCtrl.SpawnPoints.CellColorCtrls[_checkRandoms[_rand]];
    }

    protected virtual void LoadChecckRandom()
    {
        _checkRandoms = new List<int>();
        for (int i = 0; i < this.colorSpawnCtrl.SpawnPoints.CellColorCtrls.Count; i++)
        {
            if (this.colorSpawnCtrl.SpawnPoints.CellColorCtrls[i].PetColorCtrls.Count > 0) continue;
            this._checkRandoms.Add(i);
        }
    }
}
