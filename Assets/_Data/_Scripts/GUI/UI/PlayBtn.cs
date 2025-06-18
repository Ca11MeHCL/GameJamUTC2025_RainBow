using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBtn : MonoBehaviour
{
    public void PlayGame()
    {
        SceneController.Instance.PlayGame();
    }
}
