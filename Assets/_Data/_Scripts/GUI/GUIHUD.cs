using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class GUIHUD : GUIBase,MMEventListener<EDataChanged>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    private void Awake()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text is not assigned!");
        }
    }

    private void OnEnable()
    {
        this.MMEventStartListening<EDataChanged>();
        
    }
    private void OnDisable()
    {
        this.MMEventStopListening<EDataChanged>();
    }

    public void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {DataManager.Instance.Score}";
        }
        if(levelText != null){
            levelText.text = $"Level {DataManager.Instance.LevelId}";
        }
    }

    public void OnMMEvent(EDataChanged eventType)
    {
        UpdateScore();
    }
}