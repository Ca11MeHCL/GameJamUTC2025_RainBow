using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, MMEventListener<EEndLevel>
{
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private float spawnInterval = 2.0f;

    private List<float> spawnedEnemyPositions = new List<float>();
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Animator animator;

    private void Start()
    {
        if (levelManager == null)
        {
            Debug.LogError("LevelManager chưa được gán hoặc không có trên cùng GameObject.");
            return;
        }

        LoadCurrentLevelData();
    }

    private void OnEnable()
    {
        this.MMEventStartListening<EEndLevel>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<EEndLevel>();
    }

    private void LoadCurrentLevelData()
    {
        LevelData currentLevel = levelManager.CurrentLevel;
        if (currentLevel == null)
        {
            Debug.LogError("Level hiện tại null.");
            return;
        }

        if (currentLevel.rowPositions == null || currentLevel.rowPositions.Count == 0)
        {
            Debug.LogError("rowPositions trong LevelData trống.");
            return;
        }

        spawnInterval = currentLevel.spawnInterval;
        spawnedEnemyPositions = new List<float>(currentLevel.rowPositions);

        StartCoroutine(PlayClip(animator, "appear"));
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        LevelData currentLevel = levelManager.CurrentLevel;
        if (currentLevel == null || currentLevel.enemiesToSpawn == null)
        {
            yield break;
        }

        foreach (var enemyInfo in currentLevel.enemiesToSpawn)
        {
            for (int i = 0; i < enemyInfo.spawnCount; i++)
            {
                if (animator != null)
                {
                    animator.enabled = true;
                    animator.Play("Idle", 0, 0f);
                    yield return null;

                    float idleLength = animator.GetCurrentAnimatorStateInfo(0).length;
                    yield return new WaitForSeconds(idleLength);
                }

                SpawnEnemy(enemyInfo.enemyPrefab);
                yield return new WaitForSeconds(spawnInterval);
            }
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

    private void SpawnEnemy(GameObject prefab)
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

        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity, enemyContainer.transform);
        AudioManager.Instance.PlayPopSound();
        var enemyCtrl = enemy.GetComponent<EnemyController>();
        if (enemyCtrl != null)
        {
            enemyCtrl.PlaySpawnAnimation();
            enemyCtrl.MoveTo(targetPosition);
        }
    }

    private IEnumerator PlayClip(Animator eanimator, string clipName)
    {
        eanimator.Play(clipName);
        yield return new WaitForSeconds(eanimator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void OnMMEvent(EEndLevel eventType)
    {
        Debug.Log("EnemySpawner nhận sự kiện kết thúc level.");
        StartCoroutine(WaitAndLoadNextLevel());
    }

    private IEnumerator WaitAndLoadNextLevel()
    {
        yield return new WaitForSeconds(3f); // ⏳ Nghỉ 3 giây để tạo level mới xong

        LoadCurrentLevelData(); // Sau đó mới load enemy từ level mới
    }

}
