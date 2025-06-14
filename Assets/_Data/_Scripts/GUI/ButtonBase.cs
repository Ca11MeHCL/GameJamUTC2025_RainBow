using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBase : MonoBehaviour
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

    public void OpenChildren(Image image)
    {
        image.enabled = true; 
        foreach (Transform child in image.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void CloseChildren(Image image)
    {
        image.enabled = false; 
        foreach (Transform child in image.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}