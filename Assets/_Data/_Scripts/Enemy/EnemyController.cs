using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2.0f; // Movement speed
    [SerializeField] private float jumpHeight = 2.0f; // Maximum height of the jump
    [SerializeField] private float jumpDuration = 0.5f; // Time it takes to complete the jump
    
    [Header("Values Settings")]
    [SerializeField] private int score = 100; // Score to be awarded when the enemy dies
    public int Score => score; 
    [SerializeField] private int energy = 1; // Energy to be awarded when the enemy dies
    public int Energy => energy;
    private bool isJumping = true; // Check if the enemy is already jumping
    private Animator animator;
    CloudController cloud;

    #region MonoBehaviour
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
            checkOutScreen();
        }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Cloud") || other.CompareTag("Enemy"))
        if (other.CompareTag("Cloud"))
        {
            speed = 0.5f;
            cloud = other.GetComponentInChildren<CloudController>();
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Cloud"))
        {
            if (cloud.currentHP == 0) this.speed = 2f;
        }
    }*/

    #endregion
    #region Public Methods

    public void PlaySpawnAnimation()
    {
        StartCoroutine(PlaySpawnAnimationCoroutine());
    }
    public void MoveTo(Vector3 targetPos)
    {
        transform.DOMove(targetPos, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            PlayIdleLoop();
        });
    }
    private void PlayIdleLoop()
    {
        if (animator != null)
        {
            animator.Play("Idle");
        }
    }
    public void PlayDieVFX()
    {
        StartCoroutine(PlayDieVFXCoroutine());
      
    }
    #endregion
    #region Private Methods

    private void checkOutScreen()
        {
            if(transform.position.x < -10f) 
            {
                Destroy(gameObject); 
            }
        }
    private IEnumerator PlaySpawnAnimationCoroutine()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator not found on " + gameObject.name);
            yield break;
        }

        animator.Play("Spawn");

        // Chờ animation "Spawn" chạy xong
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Sau đó chuyển sang "Idle"
        animator.Play("Idle");
    }
    private IEnumerator PlayDieVFXCoroutine()
    {
        if (animator != null)
        {
            animator.Play("Die");
            Debug.Log("bat coroutine die");
            // Wait until the "Die" animation finishes
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
        Destroy(gameObject);
    }
    #endregion
    
    #region Jump Handler Methods

    public void Jump(Vector3 startPosition, Vector3 targetPosition)
    {
        if (isJumping)
        {
            StartCoroutine(JumpCoroutine(startPosition, targetPosition));
        }
    }

    private IEnumerator JumpCoroutine(Vector3 startPosition, Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation factor
            float t = elapsedTime / jumpDuration;

            // Interpolate position horizontally
            float x = Mathf.Lerp(startPosition.x, targetPosition.x, t);
            float y = Mathf.Lerp(startPosition.y, targetPosition.y, t);

            // Add a sine wave for vertical jump effect
            y += Mathf.Sin(t * Mathf.PI) * jumpHeight;

            transform.position = new Vector3(x, y, startPosition.z);

            yield return null;
        }

        // Ensure the enemy lands at the target position
        transform.position = targetPosition;
        isJumping = false;
    }
    #endregion
}