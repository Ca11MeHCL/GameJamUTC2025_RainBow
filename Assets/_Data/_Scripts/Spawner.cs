using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : ImpBehaviour
{
    [Header("Spawner")]
    [SerializeField] protected Transform holder;

    [SerializeField] protected List<Transform> prefabs;
    [SerializeField] protected List<Transform> poolObjs;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPrefabs();
        this.LoadHolder();
    }

    protected virtual void LoadPrefabs()
    {
        if (this.prefabs.Count > 0) return;

        Transform _prefabObj = transform.Find("Prefabs");
        foreach (Transform prefab in _prefabObj)
        {
            this.prefabs.Add(prefab);
        }

        //this.HidePrefabs();

        Debug.Log(transform.name + ": LoadPrefabs", gameObject);
    }

    protected virtual void LoadHolder()
    {
        if (this.holder != null) return;
        this.holder = transform.Find("Holder");
        Debug.Log(transform.name + ": LoadHolder", gameObject);
    }

    //protected virtual void HidePrefabs()
    //{
    //    foreach (Transform prefab in this.prefabs)
    //    {
    //        prefab.gameObject.SetActive(false);
    //    }
    //}

    public virtual Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        Transform _prefab = this.GetPrefabByName(prefabName);
        if (_prefab == null)
        {
            Debug.LogWarning("Prefab not found: " + prefabName);
            return null;
        }

        return this.Spawn(_prefab, spawnPos, rotation);
    }

    public virtual Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform _newPrefab = this.GetObjectFromPool(prefab);
        _newPrefab.SetPositionAndRotation(spawnPos, rotation);

        _newPrefab.parent = this.holder;
        return _newPrefab;
    }

    public virtual Transform GetPrefabByName(string prefabName)
    {
        foreach (Transform prefab in this.prefabs)
        {
            if (prefab.name == prefabName) return prefab;
        }

        return null;
    }

    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach (Transform poolObj in this.poolObjs)
        {
            if (poolObj.name == prefab.name)
            {
                this.poolObjs.Remove(poolObj);
                return poolObj;
            }
        }

        Transform _newPrefab = Instantiate(prefab);
        _newPrefab.name = prefab.name;
        return _newPrefab;
    }

    public virtual void Despawn(Transform obj)
    {
        this.poolObjs.Add(obj);
        obj.gameObject.SetActive(false);
        obj.SetParent(this.holder);
    }

    public virtual Transform RandomPrefab()
    {
        int _rand = Random.Range(0, this.prefabs.Count);
        return this.prefabs[_rand];
    }
}
