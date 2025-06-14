using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeadMenuCtrl : MonoBehaviour
{
    [SerializeField] GameObject deadMenu;
    [SerializeField] TextMeshProUGUI txtScore;

    public void Dead()
    {
        deadMenu.SetActive(true);
        txtScore.text = DataManager.Instance.Score.ToString();
        Time.timeScale = 0;
    }
}
