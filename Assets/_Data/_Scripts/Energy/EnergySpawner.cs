using MoreMountains.Tools;
using UnityEngine;

public class EnergySpawner : MonoBehaviour,MMEventListener<EEnemyDie>
{
    [SerializeField] private GameObject energyPrefab;

    private void OnEnable()
    {
        this.MMEventStartListening<EEnemyDie>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<EEnemyDie>();
    }
    private void SpawnCoin(Vector3 position,int energy)
    {
        for (int i = 0; i < energy; i++)
        {
            //Instantiate(energyPrefab, position, Quaternion.identity);
            GameObject energyObject = PoolManager.Instance.SpawnFromPool(PoolManager.TagType.energy, position, Quaternion.identity);
        }
        
    }

    public void OnMMEvent(EEnemyDie eventType)
    {
        SpawnCoin(eventType.position,  eventType.energy);
    }
}