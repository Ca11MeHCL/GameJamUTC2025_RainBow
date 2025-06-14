using UnityEngine;

public class AttackController : MonoBehaviour
{
    public int damage = 100; // Damage dealt
    public enum targetType
    {
        Enemy,
        Color
    }
    [SerializeField] private targetType target;

    private Animator animator;
    private bool hasHit = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        hasHit = false;
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
        if (hasHit) return; // Ensure only one hit is processed

        if (target == targetType.Enemy&& collision.CompareTag(target.ToString()))
        {
            var health = collision.GetComponent<HealthController>();
            if (health != null)
            {
                hasHit = true;

                // Play explosion animation
                if (animator != null)
                {
                    animator.Play("Explosion");
                }

                // Apply damage
                health.TakeDamage(damage);

                Debug.Log("Explosion animation triggered!");
            }
        }
        else if (target == targetType.Color && collision.CompareTag(target.ToString()))
        {
            animator.Play("Attack");
            Debug.Log("Attack animation triggered!");
        }
    }
}