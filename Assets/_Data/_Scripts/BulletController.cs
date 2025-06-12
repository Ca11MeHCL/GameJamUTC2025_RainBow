using System;
using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 5f;
    public float rowTolerance = 1f; // Sai số cho phép để coi là "cùng hàng"
    private Transform targetEnemy;
    private bool isReadyToShoot = false;
    private bool isExploding = false;

  

    private void Start()
    {
        StartCoroutine(JumpUpThenShoot());
    }

    private void Update()
    {
        if (!isReadyToShoot || isExploding) return;

        if (targetEnemy != null)
        {
            Vector3 dir = (targetEnemy.position - transform.position).normalized;
            transform.position += speed * Time.deltaTime * dir;
        }
        else
        {
            transform.position += speed * Time.deltaTime * Vector3.right;
        }
    }

    private IEnumerator JumpUpThenShoot()
    {
        float jumpHeight = 2.5f;
        float bounceDownOffset = 1f;
        float jumpUpDuration = 0.2f;
        float bounceDownDuration = 0.2f;

        Vector3 startPos = transform.position;
        Vector3 peakPos = startPos + Vector3.up * jumpHeight;
        Vector3 settlePos = startPos + Vector3.up * (jumpHeight - bounceDownOffset);

        // Phase 1: Nhảy lên
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / jumpUpDuration;
            transform.position = Vector3.Lerp(startPos, peakPos, t);
            yield return null;
        }

        // Phase 2: Rơi xuống nhẹ
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / bounceDownDuration;
            transform.position = Vector3.Lerp(peakPos, settlePos, t);
            yield return null;
        }
        

        FindEnemyInSameRow();
        isReadyToShoot = true;
    }


    private void FindEnemyInSameRow()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float yDiff = Mathf.Abs(enemy.transform.position.y - transform.position.y);
            float xDiff = enemy.transform.position.x - transform.position.x;

            if (yDiff < rowTolerance && xDiff > 0)
            {
                float distance = xDiff;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetEnemy = enemy.transform;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isExploding) return;

        if (other.CompareTag("Enemy"))
        {
            isExploding = true;
            StartCoroutine(PlayExplosionThenDestroy());
        }
    }

    private IEnumerator PlayExplosionThenDestroy()
    {
       
        Debug.Log("Explosion effect played.");
        yield return new WaitForSeconds(0.5f); // Thời gian chờ trước khi hủy đối tượng

        Destroy(gameObject);
    }
}
