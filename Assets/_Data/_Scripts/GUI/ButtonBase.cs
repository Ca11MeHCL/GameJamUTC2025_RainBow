using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
  
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        if (AudioManager.Instance != null )
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(PlayClickSound);
    }
}