using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 2.0f; // Movement speed
    public float Speed {  get { return speed; } set { speed = value; } }

    [SerializeField] private float jumpHeight = 2.0f; // Maximum height of the jump
    [SerializeField] private float jumpDuration = 0.5f; // Time it takes to complete the jump
    private bool isJumping = true; // Check if the enemy is already jumping
    CloudController cloud;

    private bool isStoppedBySun = false;
    public bool IsStoppedBySun { get { return isStoppedBySun; } set { isStoppedBySun = value; } }

    #region MonoBehaviour 

    void Update()
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
            checkOutScreen();
        }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cloud"))
        {
            cloud = other.GetComponentInChildren<CloudController>();
            if (cloud != null && cloud.currentHP > 0)
            {
                speed = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isStoppedBySun) return;
        if (other.CompareTag("Cloud"))
        {
            speed = 2f;
            cloud = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isStoppedBySun) return;
        if (collision.CompareTag("Cloud"))
        {
            if (cloud != null && cloud.currentHP == 0)
            {
                speed = 2f;
            }
        }
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