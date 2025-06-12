using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using UnityEngine;

public class EnemySpawner : MonoBehaviour,MMEventListener<EEndLevel>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private float spawnInterval = 2.0f;
    [SerializeField] private int numberOfEnemies = 3;
    private int spawnedEnemiesCount = 0;

    [SerializeField] private Animator animator;
    [SerializeField] private LevelManager levelManager; 
    [SerializeField] private BoardController boardController;
    private List<float> spawnedEnemyPositions = new List<float>();
    private void Start()
    {
        if (levelManager == null)
        {
            Debug.LogError("LevelManager chưa được gán trong EnemySpawner.");
            return;
        }

        LevelData currentLevel = levelManager.CurrentLevel;
        if (currentLevel == null)
        {
            Debug.LogError("LevelManager không có level hợp lệ.");
            return;
        }

        if (currentLevel.rowPositions == null || currentLevel.rowPositions.Count == 0)
        {
            Debug.LogError("rowPositions trong LevelData đang trống.");
            return;
        }

        spawnInterval = currentLevel.spawnInterval;
        numberOfEnemies = currentLevel.numberOfEnemies;
        spawnedEnemyPositions = new List<float>(currentLevel.rowPositions); // clone

        StartCoroutine(SpawnEnemies());
    }
    private void OnEnable()
    {
        this.MMEventStartListening<EEndLevel>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<EEndLevel>();
    }



    private IEnumerator SpawnEnemies()
    {
        while (spawnedEnemiesCount < numberOfEnemies)
        {
            if (animator != null)
            {
                animator.enabled = true;
                animator.Play("Idle", 0, 0f);

                
                yield return null;

                float idleLength = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(idleLength);
            }

            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        if (animator != null)
        {
            animator.Play("Disappear");

            yield return null;

            float disappearLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(disappearLength);

            animator.enabled = false;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnedEnemyPositions == null || spawnedEnemyPositions.Count == 0)
        {
            Debug.LogWarning("spawnedEnemyPositions không có dữ liệu.");
            return;
        }

        Vector3 spawnPosition = enemyContainer.transform.position + new Vector3(0, 0.5f, 0);
        float x = spawnPosition.x + Random.Range(-2f, -1f);

        spawnedEnemyPositions = spawnedEnemyPositions.OrderBy(_ => Random.value).ToList();
        float y = spawnedEnemyPositions[0];
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

        animator.speed = -1f;         
        animator.Play(clipName, 0, 1f);      
        // Đợi clip chạy ngược xong
        float clipLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name == clipName)?.length ?? 0f;
        yield return new WaitForSeconds(clipLength);
        animator.speed = 1f;                
    }

    public void OnMMEvent(EEndLevel eventType)
    {
        // Tăng level hiện tại trong levelManager
        levelManager.GoToNextLevel(); // Bạn cần đảm bảo hàm này đã có trong LevelManager

        // Reset các biến liên quan
        spawnedEnemiesCount = 0;

        // Lấy dữ liệu level mới
        LevelData currentLevel = levelManager.CurrentLevel;
        if (currentLevel == null)
        {
            Debug.LogError("LevelManager không có level hợp lệ.");
            return;
        }

        spawnInterval = currentLevel.spawnInterval;
        numberOfEnemies = currentLevel.numberOfEnemies;
        spawnedEnemyPositions = new List<float>(currentLevel.rowPositions);

        // Bắt đầu spawn enemy mới cho level tiếp theo
        StartCoroutine(SpawnEnemies());
    }

}
