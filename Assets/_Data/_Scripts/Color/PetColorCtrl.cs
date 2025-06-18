using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetColorCtrl : ImpBehaviour
{
    [Header("PetColorCtrl")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }

    [SerializeField] protected Animator animator;
    public Animator Animator { get => animator; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpriteRenderer();
        this.LoadAnimator();
    }

    protected virtual void LoadSpriteRenderer()
    {
        if (this.spriteRenderer != null) return;
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Debug.Log(transform.name + ": LoadSpriteRenderer", gameObject);
    }

    protected virtual void LoadAnimator()
    {
        if (this.animator != null) return;
        this.animator = GetComponentInChildren<Animator>();
        Debug.Log(transform.name + ": LoadAnimator", gameObject);
    }

    protected virtual void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void PlayDropDown(float duration = 0.3f)
    {
        Vector3 startPos = transform.position;
        Vector3 fromPos = new Vector3(startPos.x, startPos.y + 1f, startPos.z);

        transform.position = fromPos;
        transform.DOMoveY(startPos.y, duration).SetEase(Ease.OutQuad);
    }
}
