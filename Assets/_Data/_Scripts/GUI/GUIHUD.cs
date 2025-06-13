using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

public class GUIHUD : GUIBase,MMEventListener<EDataChanged>
{
    [SerializeField] private TextMeshProUGUI scoreText;

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

    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    public void OnMMEvent(EDataChanged eventType)
    {
        UpdateScore(DataManager.Instance.Score);
    }
}