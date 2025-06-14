using UnityEngine;
using UnityEngine.UI;

public class UIImageAnimator : MonoBehaviour
{
    public Sprite[] frames; // Các frame của animation Idle
    public float frameRate = 12f; // 12 FPS
    private Image image;
    private int currentFrame;
    private float timer;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            image.sprite = frames[currentFrame];
            timer = 0f;
        }
    }
}