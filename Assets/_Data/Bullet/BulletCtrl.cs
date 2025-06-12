using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletCtrl : ImpBehaviour
{
    [Header("BulletCtrl")]
    [SerializeField] protected float bulletSpeed = 5f;
    [SerializeField] private float spinSpeed = 360f;
    [SerializeField] private Vector3 targetPoint;

    private Vector3 startPoint;
    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero; // Reset velocity mỗi lần spawn
        transform.localScale = Vector3.zero;
        startPoint = transform.position;

        // Xoay khi enable
        DoSpawnEffect();
    }

    private void Update()
    {
        // Xoay tròn liên tục
        transform.Rotate(Vector3.back * spinSpeed * Time.deltaTime * 2f);
    }

    protected virtual void DoSpawnEffect()
    {
        // Scale to lên
        transform.DOScale(Vector3.one * 0.3f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            if (targetPoint != null)
            {
                // Dùng đường cong (tween liên tục thay vì 2 bước để không bị khựng)
                Vector3 controlPoint = new Vector3((startPoint.x + targetPoint.x) / 2f, startPoint.y + 0.5f, (startPoint.z + targetPoint.z) / 2f);

                float duration = 1f;
                float t = 0f;

                // Di chuyển theo đường cong bằng DOTween Update
                DOTween.To(() => t, x => {
                    t = x;
                    Vector3 curvedPos = CalculateBezierPoint(t, startPoint, controlPoint, targetPoint);
                    transform.position = curvedPos;
                }, 1f, duration).SetEase(Ease.InOutQuad).OnComplete(() =>
                {
                    // Sau khi đáp xuống điểm target → dùng rigidbody để bay ngang
                    rb.velocity = Vector3.right * bulletSpeed;
                });
            }
        });
    }

    private void FixedUpdate()
    {
        // Nếu ra khỏi màn hình thì despawn
        if (Mathf.Abs(transform.position.x) > 15f) // Điều chỉnh screen limit nếu cần
        {
            rb.velocity = Vector3.zero;
            ColorBulletSpawner.Instance.Despawn(this.transform);
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.targetPoint = target;
    }

    // Tính điểm trên đường cong bezier bậc 2
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        return u * u * p0 + 2 * u * t * p1 + t * t * p2;
    }
}
