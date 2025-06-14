using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnergyController : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition = new Vector3(7.51f, -3.98f, 0f);
    [SerializeField] private float moveDuration = 2f;

    private void Start()
    {
        StartCoroutine(PlayIdle());
      
    }

    private IEnumerator PlayIdle()
    {
        yield return new WaitForSeconds(1f); // Đợi 1 giây trước khi bắt đầu di chuyển
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            { 
                EnergyManager.Instance?.Add(1);
                Destroy(gameObject);
            }); // tự hủy sau khi bay đến nơi
    }
}