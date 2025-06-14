using System;
using MoreMountains.Tools;
using UnityEngine;

public class DataManager : MonoBehaviour,MMEventListener<EEnemyDie>,MMEventListener<EEndLevel>
{
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("Singleton instance has not been created yet!");
            return _instance;
        }
    }

    [SerializeField] private int score = 0;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            MMEventManager.TriggerEvent(new EDataChanged());
        }
    }

    [SerializeField] private int levelId = 1;
    public int LevelId
    {
        get => levelId;
        set
        {
            levelId = value;
            MMEventManager.TriggerEvent(new EDataChanged());
            Debug.Log($"Level ID updated: {levelId}");
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Make persistent across scenes
            return;
        }

        if (_instance != this)
        {
            Debug.LogWarning("Another instance of DataManager already exists!");
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
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
        Score += eventType.score;
      Debug.Log($"Score updated: {eventType.score}");
    }

    public void OnMMEvent(EEndLevel eventType)
    {
        LevelId = LevelManager.Instance.currentLevelIndex;
    }
}