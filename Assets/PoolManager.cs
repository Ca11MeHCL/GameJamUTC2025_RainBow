using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public TagType tag;
        public GameObject prefab;
        public int size;
    }

    public enum TagType
    {
        enemy,
        cloud,
        energy,
        miniboss1,   // thêm mới
        miniboss2
    }

    [Header("Pool Settings")]
    [SerializeField] private List<Pool> pools;
    private Dictionary<TagType, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        poolDictionary = new Dictionary<TagType, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(TagType tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(obj);
        return obj;
    }
    
    public void ReturnToPool(GameObject obj, TagType tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return;
        }

        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}