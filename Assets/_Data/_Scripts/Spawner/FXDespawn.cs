using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXDespawn : ImpBehaviour
{
    [SerializeField] private float delay = 0.5f;

    private Coroutine disableCoroutine;

    private void OnEnable()
    {
        if (disableCoroutine != null)
            StopCoroutine(disableCoroutine);

        disableCoroutine = StartCoroutine(DisableAfterDelay());
    }

    private IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        FXSpawner.Instance.Despawn(this.transform);
    }

    private void OnDisable()
    {
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
            disableCoroutine = null;
        }
    }
}
