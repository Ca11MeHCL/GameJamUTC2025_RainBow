using UnityEngine;
using UnityEngine.UI;

public class ChangeSpriteOnClick : MonoBehaviour
{
    public Sprite sprite1;  
    public Sprite sprite2;  

    private Image imgSource;
    private bool isOriginal = true;

    void Start()
    {
        imgSource = GetComponent<Image>();

        if (imgSource != null && sprite1 != null)
        {
            imgSource.sprite = sprite1;
        }
    }
    public void ChangeAudioButton()
    {
        if (imgSource == null || sprite1 == null || sprite2 == null)
            return;

        imgSource.sprite = isOriginal ? sprite2 : sprite1;
        isOriginal = !isOriginal;
        Debug.Log("d");
    }


}
