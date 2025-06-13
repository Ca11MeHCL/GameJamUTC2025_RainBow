using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : ImpBehaviour
{
    [Header("CloudController")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }

    [SerializeField] protected Collider2D col;
    public Collider2D Collider2D { get => col; }

    [Header("Flash Settings")]
    public float flashTime = 0f;
    public float flashDuration = 3f;
    public float blinkInterval = 0.5f;

    [Header("HP Settings")]
    public int maxHP = 10;
    public int currentHP;
    protected float damageInterval = 0.5f;
    protected float healInterval = 1f;
    protected float timeSinceLastEnemy = 0f;
    protected bool isFlashing = false;

    protected Vector3 initialScale;
    protected Coroutine flashRoutine;
    protected HashSet<Collider2D> enemiesInside = new HashSet<Collider2D>();        // Trạng thái đang nhấp nháy

    public bool isEnable = true;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpriteRenderer();
        this.LoadCollider2D();
    }

    protected virtual void LoadSpriteRenderer()
    {
        if (this.spriteRenderer != null) return;
        this.spriteRenderer = GetComponentInParent<SpriteRenderer>();
        Debug.Log(transform.name + ": LoadSpriteRenderer", gameObject);
    }

    protected virtual void LoadCollider2D()
    {
        if (this.col != null) return;
        this.col = GetComponent<Collider2D>();
        Debug.Log(transform.name + ": LoadCollider2D", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        initialScale = transform.parent.localScale;
        spriteRenderer.enabled = true;
        col.enabled = true;
        currentHP = maxHP;

        StartCoroutine(DamageRoutine());
        StartCoroutine(HealRoutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInside.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInside.Remove(other);
        }
    }

    protected IEnumerator DamageRoutine()
    {
        while (true)
        {
            if (isEnable && enemiesInside.Count > 0 && !isFlashing)
            {
                currentHP -= 1;
                currentHP = Mathf.Max(0, currentHP);
                timeSinceLastEnemy = 0f;

                if (currentHP < maxHP && flashRoutine == null)
                {
                    flashRoutine = StartCoroutine(FlashEffectWhileAlive());
                }

                if (currentHP <= 0)
                {
                    if (flashRoutine != null)
                    {
                        StopCoroutine(flashRoutine);
                        flashRoutine = null;
                    }
                    StartCoroutine(FlashAndDisappear());
                    yield break;
                }
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }

    protected IEnumerator HealRoutine()
    {
        while (true)
        {
            if (enemiesInside.Count == 0 && currentHP < maxHP)
            {
                timeSinceLastEnemy += Time.deltaTime;

                if (timeSinceLastEnemy >= healInterval)
                {
                    currentHP += 1;
                    currentHP = Mathf.Min(currentHP, maxHP);
                    timeSinceLastEnemy = 0f;

                    if (currentHP == maxHP && flashRoutine != null)
                    {
                        StopCoroutine(flashRoutine);
                        SetAlpha(1f);
                        flashRoutine = null;
                    }
                }
            }
            else
            {
                timeSinceLastEnemy = 0f;
            }

            yield return null;
        }
    }

    protected IEnumerator FlashEffectWhileAlive()
    {
        while (currentHP > 0 && currentHP < maxHP)
        {
            SetAlpha(0.3f);
            yield return new WaitForSeconds(blinkInterval / 2f);
            SetAlpha(1f);
            yield return new WaitForSeconds(blinkInterval / 2f);
        }
    }

    protected IEnumerator FlashAndDisappear()
    {
        isFlashing = true;
        SetAlpha(1f);

        // Co nhỏ đối tượng
        yield return transform.parent.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).WaitForCompletion();

        // Ẩn Sprite và Collider
        spriteRenderer.enabled = false;
        col.enabled = false;

        isEnable = false; // ❗Đặt ở đây: sau khi đối tượng đã hoàn toàn biến mất khỏi màn hình và không còn va chạm

        // Đợi trước khi hồi sinh
        yield return new WaitForSeconds(3f);

        // Reset trạng thái
        currentHP = maxHP;
        flashTime = 0f;
        isFlashing = false;

        spriteRenderer.enabled = true;
        col.enabled = true;

        SetAlpha(0f);
        transform.parent.localScale = Vector3.zero;

        // Hoạt ảnh hiện lại
        spriteRenderer.DOFade(1f, 0.5f);
        yield return transform.parent.DOScale(initialScale, 0.5f).SetEase(Ease.OutBack).WaitForCompletion();

        SetAlpha(1f);
        isEnable = true; // ✅ Đặt lại thành true sau khi hiện ra

        // Khởi động lại DamageRoutine
        StartCoroutine(DamageRoutine());
    }

    protected void SetAlpha(float a)
    {
        if (spriteRenderer == null) return;
        Color c = spriteRenderer.color;
        c.a = a;
        spriteRenderer.color = c;
    }
}
