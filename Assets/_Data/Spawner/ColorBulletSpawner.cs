using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBulletSpawner : Spawner
{
    [Header("ColorBulletSpawner")]
    private static ColorBulletSpawner _instance;
    public static ColorBulletSpawner Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("Singleton instance has not been created yet!");
            return _instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (_instance == null)
        {
            _instance = this;
            if (transform.parent == null) DontDestroyOnLoad(gameObject);
            return;
        }

        if (_instance != this)
        {
            Debug.LogWarning("Another instance of ColorBulletSpawner already exists!");
            Destroy(gameObject);
            return;
        }
    }

    protected Dictionary<string, Transform> _bulletPrefabDict = new Dictionary<string, Transform>();

    protected override void Start()
    {
        base.Start();
        //this.LoadPrefabs();
        this.BuildBulletDictionary();
    }

    protected virtual void BuildBulletDictionary()
    {
        _bulletPrefabDict.Clear();

        foreach (Transform prefab in this.prefabs)
        {
            string[] nameParts = prefab.name.Split(' ');
            List<string> colors = new List<string>(nameParts);
            colors.Sort();

            string key = string.Join(" ", colors);
            if (!_bulletPrefabDict.ContainsKey(key))
            {
                _bulletPrefabDict[key] = prefab;
            }
            else
            {
                Debug.LogWarning($"Trùng key màu: {key} trong bulletPrefabs.");
            }
        }
    }

    public Transform GetBulletPrefab(List<PetColorCtrl> colors)
    {
        if (colors == null || colors.Count == 0) return null;
        List<string> names = new List<string>();

        foreach (PetColorCtrl p in colors)
        {
            names.Add(p.transform.name);
        }

        List<string> sortedColors = new List<string>(names);
        sortedColors.Sort();
        string key = string.Join(" ", sortedColors);

        if (_bulletPrefabDict.TryGetValue(key, out Transform foundPrefab))
        {
            return foundPrefab;
        }

        Debug.LogWarning($"Không tìm thấy bullet prefab cho tổ hợp màu: {key}");
        return null;
    }
}
