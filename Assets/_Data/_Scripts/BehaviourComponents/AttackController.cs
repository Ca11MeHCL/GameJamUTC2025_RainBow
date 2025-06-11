using UnityEngine;

public class AttackController : MonoBehaviour
{
    public int damage = 1;                 // Sát thương gây ra
    public string enemyTag = "Enemy";         // Tag của enemy
    private Animator animator;
    private bool hasHit = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (animator != null)
        {
            animator.Play("Shoot");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return; // Đảm bảo chỉ xử lý một lần
        if (collision.CompareTag(enemyTag))
        {
            hasHit = true;

            // Gọi animator nổ
            if (animator != null)
            {
                animator.Play("Explosion");
            }

            // Gây sát thương nếu enemy có HealthController
            var health = collision.GetComponent<HealthController>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Hủy sau thời gian animation (tuỳ thời lượng clip "Explosion")
            Destroy(gameObject, 0.3f);
        }
    }
}