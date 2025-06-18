using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBase : MonoBehaviour
{
    // Start is called before the first frame update
    private Image image;
    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false;
        foreach (Transform child in image.transform)
        {
            child.gameObject.SetActive(false);
        }
    }

}
