using System.Collections;
using UnityEngine;

public class CellController : MonoBehaviour
{
    private int x;
    public int xPos
    {
        get => x;
        set => x = value;
    }

    private int y;
    public int yPos
    {
        get => y;
        set => y = value;
    }

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject bulletPrefab;       // Prefab viên đạn
    [SerializeField] private float shootInterval = 2.0f;    // Khoảng thời gian giữa các lần bắn

    private void Start()
    {
        if (animator != null)
        {
            animator.Play("Loop");
        }

        if (transform.childCount > 0)
        {
            StartCoroutine(ShootContinuously());
        }
    }

    private IEnumerator ShootContinuously()
    {
        while (true)
        {
            SpawnBullet();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    private void SpawnBullet()
    {
        Vector3 spawnPos = transform.position + new Vector3(0.5f, 0.25f, 0); // chỉnh vị trí bắn ra nếu cần
        Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
    }
}