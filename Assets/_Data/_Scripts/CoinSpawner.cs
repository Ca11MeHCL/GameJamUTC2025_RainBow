using MoreMountains.Tools;
using UnityEngine;

public class CoinSpawner : MonoBehaviour,MMEventListener<EEnemyDie>
{
    [SerializeField] private GameObject coinPrefab;

    private void OnEnable()
    {
        this.MMEventStartListening<EEnemyDie>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<EEnemyDie>();
    }
    private void SpawnCoin(Vector3 position)
    {
        Instantiate(coinPrefab, position, Quaternion.identity);
    }

    public void OnMMEvent(EEnemyDie eventType)
    {
        SpawnCoin(eventType.position);
    }
}