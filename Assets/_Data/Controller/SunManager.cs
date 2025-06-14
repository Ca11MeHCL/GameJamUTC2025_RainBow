using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SunManager : ImpBehaviour
{
    [Header("SunManager")]

    [SerializeField] protected int sun = 8;
    public int Sun { get => sun; }

    [SerializeField] protected GameObject _sunPoint;

    [SerializeField] protected List<SunController> sunControllers;
    public List<SunController> SunControllers { get => sunControllers; }

    private Dictionary<float, float> lastSunActivationTimes = new Dictionary<float, float>();
    private float sunCooldown = 5f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSunControllers();
    }

    protected virtual void LoadSunControllers()
    {
        this.sunControllers.Clear();
        foreach (Transform _point in _sunPoint.transform)
        {
            SunController _cell = _point.GetComponent<SunController>();
            this.sunControllers.Add(_cell);
        }
        Debug.Log(transform.name + ": LoadSunControllers", gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            float enemyY = Mathf.Round(enemy.transform.position.y * 10f) / 10f;

            enemy.IsStoppedBySun = true;
            enemy.Speed = 0f;

            if (lastSunActivationTimes.ContainsKey(enemyY))
            {
                float lastTime = lastSunActivationTimes[enemyY];
                if (Time.time - lastTime < sunCooldown)
                {
                    return;
                }
            }

            lastSunActivationTimes[enemyY] = Time.time;

            Vector3 targetPosition = new Vector3(-9.2f, enemy.transform.position.y, 0f);

            if (sunControllers.Count > 0)
            {
                SunController lastSun = sunControllers[sunControllers.Count - 1];
                if (lastSun != null)
                {
                    lastSun.SunActive(targetPosition);
                    sunControllers.RemoveAt(sunControllers.Count - 1);
                }
            }
        }
    }
}
