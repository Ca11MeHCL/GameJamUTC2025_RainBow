using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBtn : MonoBehaviour
{
    public void PlayGame()
    {
        AudioManager.Instance.PlayButtonClick();
        SceneController.Instance.PlayGame();
    }
}
