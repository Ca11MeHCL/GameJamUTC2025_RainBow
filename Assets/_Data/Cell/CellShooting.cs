using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellShooting : ImpBehaviour
{
    [Header("CellShooting")]
    [SerializeField] protected CellColorCtrl cellColorCtrl;
    public CellColorCtrl CellColorCtrl { get => cellColorCtrl; }

    [SerializeField] protected int bulletNumber = 1;
    [SerializeField] protected float shootingSpeed = 1f;

    private bool _isShooting = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCellColorCtrl();
    }

    protected virtual void LoadCellColorCtrl()
    {
        if (cellColorCtrl != null) return;
        this.cellColorCtrl = GetComponent<CellColorCtrl>();
        Debug.Log(transform.name + ": LoadCellColorCtrl", gameObject);
    }

    private void Update()
    {
        this.Shooting();
    }

    protected virtual void Shooting()
    {
        if (!this.CheckHasEnemy()) return;
        if (!_isShooting && this.cellColorCtrl.PetColorCtrls.Count > 0)
        {
            this.cellColorCtrl.PetColorCtrls[this.cellColorCtrl.PetColorCtrls.Count - 1].Animator.Play("Pop");
            StartCoroutine(ShootBulletsCoroutine());
        }
    }

    protected virtual IEnumerator ShootBulletsCoroutine()
    {
        _isShooting = true;

        Vector3 pos = this.cellColorCtrl.PetColorCtrls[this.cellColorCtrl.PetColorCtrls.Count - 1].transform.position + new Vector3(0, 0.8f, 0);
        Quaternion rot = transform.rotation;

        Transform prefab = ColorBulletSpawner.Instance.GetBulletPrefab(this.cellColorCtrl.PetColorCtrls);
        Transform obj = ColorBulletSpawner.Instance.Spawn(prefab, pos, rot);
        obj.GetComponent<BulletCtrl>().SetTarget(this.transform.position + new Vector3(1f, 0.4f, 0));
        obj.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.4f);
        if (this.cellColorCtrl.PetColorCtrls.Count > 0)
        {
            this.cellColorCtrl.PetColorCtrls[this.cellColorCtrl.PetColorCtrls.Count - 1].Animator.Play("Idle");
        }

        yield return new WaitForSeconds(shootingSpeed - 0.4f);

        _isShooting = false;
    }

    protected virtual bool CheckHasEnemy()
    {
        float rowY = transform.position.y + 0.4f;
        float startX = transform.position.x + 0.5f;
        float endX = 15f;

        // Tính center và size của vùng quét
        float rangeX = Mathf.Abs(endX - startX);
        float checkHeight = 0.5f;

        // Tính tâm hình chữ nhật để quét
        float centerX = (startX + endX) / 2f;
        Vector2 checkCenter = new Vector2(centerX, rowY);
        Vector2 checkSize = new Vector2(rangeX, checkHeight);

        // Quét collider trong vùng
        Collider2D[] hits = Physics2D.OverlapBoxAll(checkCenter, checkSize, 0f);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }
}
