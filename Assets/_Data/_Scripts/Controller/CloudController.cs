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

    [SerializeField] protected ColorSpawnPointsCtrl colorSpawnPointsCtrl;
    public ColorSpawnPointsCtrl ColorSpawnPointsCtrl { get => colorSpawnPointsCtrl; }

    [SerializeField] protected CellColorCtrl cellColorCtrl;

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
    protected HashSet<Collider2D> enemiesInside = new HashSet<Collider2D>();

    //public bool isEnable = true;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpriteRenderer();
        this.LoadCollider2D();
        this.LoadColorSpawnPointsCtrl();
        this.LoadCellColorCtrl();
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

    protected virtual void LoadColorSpawnPointsCtrl()
    {
        if (this.colorSpawnPointsCtrl != null) return;
        this.colorSpawnPointsCtrl = GameObject.Find("ColorSpawnPoints").GetComponent<ColorSpawnPointsCtrl>();
        Debug.Log(transform.name + ": LoadColorSpawnPointsCtrl", gameObject);
    }

    protected virtual void LoadCellColorCtrl()
    {
        if (this.cellColorCtrl != null) return;
        foreach (CellColorCtrl c in this.colorSpawnPointsCtrl.CellColorCtrls)
        {
            if (c.gameObject.name == this.transform.parent.gameObject.name)
            {
                this.cellColorCtrl = c;
                break;
            }
        }
        Debug.Log(transform.name + ": LoadCellColorCtrl", gameObject);
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

    private void CleanInvalidEnemies()
    {
        enemiesInside.RemoveWhere(enemy => enemy == null || !enemy.gameObject.activeInHierarchy);
    }

    protected IEnumerator DamageRoutine()
    {
        while (true)
        {
            CleanInvalidEnemies();

            if (enemiesInside.Count > 0 && !isFlashing)
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

        enemiesInside.Clear();

        //isEnable = false;
        this.DisableCell();

        // Đợi trước khi hồi sinh
        yield return new WaitForSeconds(3f);

        // Reset trạng thái
        currentHP = maxHP;
        flashTime = 0f;
        isFlashing = false;

        spriteRenderer.enabled = true;

        SetAlpha(0f);
        transform.parent.localScale = Vector3.zero;

        // Hoạt ảnh hiện lại
        spriteRenderer.DOFade(1f, 0.5f);
        yield return transform.parent.DOScale(initialScale, 0.5f).SetEase(Ease.OutBack).WaitForCompletion();

        col.enabled = true;
        SetAlpha(1f);
        //isEnable = true;
        this.EnableCell();

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
    protected virtual void EnableCell()
    {
        this.cellColorCtrl.gameObject.SetActive(true);
        this.colorSpawnPointsCtrl.LoadActiveCellColorCtrls();
    }

    protected virtual void DisableCell()
    {
        if (this.cellColorCtrl.PetColorCtrls.Count == 0)
        {
            StartCoroutine(WaitForDespawn(0f));
            return;
        }

        Vector3 pivot = this.cellColorCtrl.PetColorCtrls[0].transform.position;
        float rotateDuration = 0.3f;
        float shrinkDuration = 0.5f;
        for (int i = 0; i < this.cellColorCtrl.PetColorCtrls.Count; i++)
        {
            Transform t = this.cellColorCtrl.PetColorCtrls[i].transform;
            GameObject obj = t.gameObject;

            // Tính góc xoay ngẫu nhiên
            float angle = Random.Range(0f, 30f);
            angle *= (i % 2 == 0) ? -1f : 1f;

            // Tính vị trí sau khi quay quanh pivot
            Vector3 dir = t.position - pivot;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 rotatedPos = pivot + rotation * dir;

            // Sequence: Di chuyển → Xoay → Thu nhỏ → Despawn
            Sequence seq = DOTween.Sequence();

            // Di chuyển đến vị trí xoay quanh pivot
            seq.Append(t.DOMove(rotatedPos, rotateDuration).SetEase(Ease.OutQuad));

            // Xoay theo trục Z
            seq.Join(t.DORotate(
                new Vector3(0, 0, t.eulerAngles.z + angle),
                rotateDuration,
                RotateMode.Fast
            ).SetEase(Ease.OutQuad));

            // Thu nhỏ sau khi quay xong
            seq.Append(t.DOScale(Vector3.zero, shrinkDuration).SetEase(Ease.InQuad));

            // Gọi despawn khi xong hiệu ứng
            seq.OnComplete(() => {
                ColorSpawner.Instance.Despawn(t);
                this.cellColorCtrl.LoadPetColorCtrl();
            });
            //ColorSpawner.Instance.Despawn(this.draggedPets[i].transform);
        }

        StartCoroutine(WaitForDespawn(rotateDuration + shrinkDuration));
    }

    private IEnumerator WaitForDespawn(float time)
    {
        yield return new WaitForSeconds(time);
        this.cellColorCtrl.gameObject.SetActive(false);
        this.colorSpawnPointsCtrl.LoadActiveCellColorCtrls();
    }
}
