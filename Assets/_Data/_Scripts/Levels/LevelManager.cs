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

        Debug.Log($"Chuyển sang level {currentLevelIndex + 1}");
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