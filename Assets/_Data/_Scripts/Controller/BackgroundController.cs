using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.1f; // Tốc độ cuộn ngang
    private Material mat;
    private Vector2 offset;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        offset = mat.mainTextureOffset;
    }

    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}