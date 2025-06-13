using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<LevelData> levels; // Gán trong Inspector
    public int currentLevelIndex = 0;

    public LevelData CurrentLevel => levels[currentLevelIndex];

    
    public void GoToNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count)
        {
            Debug.Log("chuyển sang level :" + currentLevelIndex);
            return;
        }
        else
        {
            CreateNewLevel();
        }

        Debug.Log($"Chuyển sang level {currentLevelIndex + 1}");
    }

    private void CreateNewLevel()
    {
        LevelData preLevel = levels[currentLevelIndex];
        LevelData nextLevel = ScriptableObject.CreateInstance<LevelData>();
        if (currentLevelIndex <= 5)
        { 
            nextLevel.numberOfEnemies = preLevel.numberOfEnemies + 3; //them số  lượng enemy
            nextLevel.spawnInterval = preLevel.spawnInterval - 0.1f; // giảm thời gian spawn
            nextLevel.rowPositions = new List<float>(preLevel.rowPositions); // giữ nguyên vị trí hàng
        }
        else
        {
            nextLevel.numberOfEnemies = preLevel.numberOfEnemies + 5; //them số  lượng enemy
            nextLevel.spawnInterval = preLevel.spawnInterval - 0.2f; // giảm thời gian spawn
            nextLevel.rowPositions = new List<float>(preLevel.rowPositions); // giữ nguyên vị trí hàng
        }
        
        levels.Add(nextLevel);
        
    }

    public void RestartLevel()
    {
        // Giữ nguyên currentLevelIndex
    }

    public void Reset()
    {
        currentLevelIndex = 0;
    }

    public void TriggerEvents()
    {
        MMEventManager.TriggerEvent(new EEndLevel());
    }
}