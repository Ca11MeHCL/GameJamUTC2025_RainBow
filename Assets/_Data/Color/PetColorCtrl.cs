using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetColorCtrl : ImpBehaviour
{
    [Header("PetColorCtrl")]
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer { get => spriteRenderer; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpriteRenderer();
    }

    protected virtual void LoadSpriteRenderer()
    {
        if (this.spriteRenderer != null) return;
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log(transform.name + ": LoadSpriteRenderer", gameObject);
    }
}
