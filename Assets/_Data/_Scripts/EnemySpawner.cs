using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private float spawnInterval = 2.0f;
    [SerializeField] private int numberOfEnemies = 5;
    private int spawnedEnemiesCount = 0;

    [SerializeField] private Animator animator;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (spawnedEnemiesCount < numberOfEnemies)
        {
            if (animator != null)
            {
                animator.enabled = true;
                animator.Play("Idle", 0, 0f);

                // Chờ frame để animator cập nhật
                yield return null;

                // Chờ animation Idle chạy xong
                float idleLength = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(idleLength);
            }

            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        if (animator != null)
        {
            animator.Play("Disappear");

            // Chờ frame để animator cập nhật
            yield return null;

            // Chờ animation Disappear chạy xong
            float disappearLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(disappearLength);

            animator.enabled = false;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition = enemyContainer.transform.position + new Vector3(0, 0.5f, 0);
        float x = spawnPosition.x + Random.Range(-2f, -1f); // sửa lại cho đúng thứ tự
        float y = Random.Range(-5f, 5f);
        Vector3 targetPosition = new Vector3(x, y, 0);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.Jump(spawnPosition, targetPosition);
        }

        spawnedEnemiesCount++;
    }

    private IEnumerator PlayAnimationReverse(string clipName)
    {
        if (animator == null) yield break;

        animator.speed = -1f;                        // Phát ngược
        animator.Play(clipName, 0, 1f);              // Bắt đầu từ cuối clip

        // Đợi clip chạy ngược xong
        float clipLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name == clipName)?.length ?? 0f;

        yield return new WaitForSeconds(clipLength);
        animator.speed = 1f;                
    }

}
