using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public PoolManager.TagType tag; // ✅ Chỉ cần tag, không cần prefab
    public int spawnCount;
}