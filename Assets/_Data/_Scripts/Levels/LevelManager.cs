using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class LevelManager : MonoBehaviour, MMEventListener<EEnemyDie>, MMEventListener<EEndLevel>
{
    public static LevelManager Instance { get; private set; }

    public List<LevelData> levels;
    public List<GameObject> enemyPrefabsMasterList;

    public int currentLevelIndex = 0;
    private int currentEnemyTypeIndex = 2;
    private int defeatedEnemies = 0;

    public LevelData CurrentLevel => levels[currentLevelIndex];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        Debug.Log("totalEnemies: " + totalEnemies + ", defeatedEnemies: " + defeatedEnemies);
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

        // Nếu level tiếp theo chưa có, tạo mới trước
        if (currentLevelIndex + 1 >= levels.Count)
        {
            Debug.Log($"Chuẩn bị tạo level mới tại vị trí {levels.Count}");
            CreateNewLevel();
        }

        currentLevelIndex++;

        if (currentLevelIndex >= levels.Count)
        {
            Debug.Log("Đã tới level cuối cùng: " + (currentLevelIndex + 1));
            return;
        }

        Debug.Log($"Chuyển sang Level {currentLevelIndex + 1}");
        DataManager.Instance.LevelId = currentLevelIndex + 1;
    }


    private void CreateNewLevel()
    {
        LevelData previousLevel = levels[currentLevelIndex];
        LevelData nextLevel = ScriptableObject.CreateInstance<LevelData>();

        nextLevel.rowPositions = new List<float>(previousLevel.rowPositions);
        nextLevel.spawnInterval = Mathf.Max(0.3f, previousLevel.spawnInterval - 0.2f);
        nextLevel.enemiesToSpawn = new List<EnemySpawnInfo>();

        foreach (var enemyInfo in previousLevel.enemiesToSpawn)
        {
            nextLevel.enemiesToSpawn.Add(new EnemySpawnInfo
            {
                enemyPrefab = enemyInfo.enemyPrefab,
                spawnCount = enemyInfo.spawnCount + 1
            });
        }

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

                currentEnemyTypeIndex++;
            }
        }

        levels.Add(nextLevel);
    }

    public void RestartLevel()
    {
        defeatedEnemies = 0;
    }

    public void Reset()
    {
        currentLevelIndex = 0;
        currentEnemyTypeIndex = 0;
        defeatedEnemies = 0;
    }

    public void OnMMEvent(EEndLevel eventType)
    {
        Debug.Log("Finish lelvel "+ (currentLevelIndex + 1));
        DataManager.Instance.LevelId = currentLevelIndex + 1;
        GoToNextLevel();
    }
}