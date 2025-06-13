using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "LevelData", menuName = "Level")]
public class LevelData : ScriptableObject
{
    public float spawnInterval;
    public List<float> rowPositions;
    public List<EnemySpawnInfo> enemiesToSpawn;
}



