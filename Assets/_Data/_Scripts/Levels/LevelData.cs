using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public float spawnInterval = 2f;
    public int numberOfEnemies = 4;

    public List<float> rowPositions = new List<float>(); // Y-coordinates để spawn
}