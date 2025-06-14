using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class LevelManager : MonoBehaviour, MMEventListener<EEnemyDie>, MMEventListener<EEndLevel>
{
    public static LevelManager Instance { get; private set; } // Singleton instance

    public List<LevelData> levels; // Danh sách level hiện tại
    public List<GameObject> enemyPrefabsMasterList; // Danh sách enemy gốc (cố định theo thứ tự)

    public int currentLevelIndex = 0;
    private int currentEnemyTypeIndex = 0; // Đánh dấu đã dùng đến loại enemy nào
    private int defeatedEnemies = 0;
    public LevelData CurrentLevel => levels[currentLevelIndex];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    private void Start()
    {
        defeatedEnemies = 0;
        this.MMEventStartListening<EEnemyDie>();
        this.MMEventStartListening<EEndLevel>();
    }

    private void OnDestroy()
    {
        this.MMEventStopListening<EEnemyDie>();
        this.MMEventStopListening<EEndLevel>();
    }

    public void OnMMEvent(EEnemyDie eventType)
    {
        defeatedEnemies++;
        CheckLevelCompletion();
    }

    private void CheckLevelCompletion()
    {
        int totalEnemies = CurrentLevel.enemiesToSpawn.Sum(e => e.spawnCount);
        if (defeatedEnemies >= totalEnemies)
        {
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        Debug.Log("Level Complete!");
        MMEventManager.TriggerEvent(new EEndLevel());
    }

    public void GoToNextLevel()
    {
        defeatedEnemies = 0;
        currentLevelIndex++;

        if (currentLevelIndex >= levels.Count)
        {
            Debug.Log("Đã tới level cuối cùng: " + currentLevelIndex);
            return;
        }

        Debug.Log($"Chuyển sang level {currentLevelIndex + 1}");
        CreateNewLevel();
    }

    private void CreateNewLevel()
    {
        LevelData previousLevel = levels[currentLevelIndex];
        LevelData nextLevel = ScriptableObject.CreateInstance<LevelData>();

        // Giữ nguyên các hàng spawn
        nextLevel.rowPositions = new List<float>(previousLevel.rowPositions);
        nextLevel.spawnInterval = Mathf.Max(0.3f, previousLevel.spawnInterval - 0.1f);

        // Copy lại enemy cũ và tăng số lượng mỗi loại
        nextLevel.enemiesToSpawn = new List<EnemySpawnInfo>();

        foreach (var enemyInfo in previousLevel.enemiesToSpawn)
        {
            nextLevel.enemiesToSpawn.Add(new EnemySpawnInfo
            {
                enemyPrefab = enemyInfo.enemyPrefab,
                spawnCount = enemyInfo.spawnCount + 1
            });
        }

        // Thêm enemy mới theo thứ tự master list
        if (currentEnemyTypeIndex < enemyPrefabsMasterList.Count)
        {
            GameObject nextEnemy = enemyPrefabsMasterList[currentEnemyTypeIndex];
            bool alreadyExists = nextLevel.enemiesToSpawn.Exists(e => e.enemyPrefab == nextEnemy);

            if (!alreadyExists)
            {
                nextLevel.enemiesToSpawn.Add(new EnemySpawnInfo
                {
                    enemyPrefab = nextEnemy,
                    spawnCount = 2
                });

                currentEnemyTypeIndex++; // tăng index cho lần sau
            }
        }

        levels.Add(nextLevel);
    }

    public void RestartLevel()
    {
        // Không reset currentLevelIndex
    }

    public void Reset()
    {
        currentLevelIndex = 0;
        currentEnemyTypeIndex = 0;
    }

    public void OnMMEvent(EEndLevel eventType)
    {
        GoToNextLevel();
    }
}