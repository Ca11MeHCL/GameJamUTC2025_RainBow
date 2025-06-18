using UnityEngine;

public class TouchCameraController : MonoBehaviour
{
    public float panSpeed = 0.5f; // Độ nhạy khi kéo
    public Vector2 minPosition;  // Giới hạn trái-dưới
    public Vector2 maxPosition;  // Giới hạn phải-trên

    private Vector3 touchStart;

    void Update()
    {
#if UNITY_EDITOR
        // Dành cho kiểm tra trên máy tính
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MoveCamera(direction);
        }
#else
        // Dành cho mobile
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = Camera.main.ScreenToWorldPoint(touch.position);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(touch.position);
                MoveCamera(direction);
            }
        }
#endif
    }

    void MoveCamera(Vector3 direction)
    {
        Vector3 newPosition = Camera.main.transform.position + direction * panSpeed;

        // Giới hạn camera trong vùng bản đồ
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);
        newPosition.z = Camera.main.transform.position.z; // Giữ nguyên z

        Camera.main.transform.position = newPosition;
    }
}
