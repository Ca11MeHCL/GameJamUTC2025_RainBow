using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : ImpBehaviour
{
    [Header("UIButton")]
    [SerializeField] protected Button btn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadButton();
    }

    protected virtual void LoadButton()
    {
        if (this.btn != null) return;
        this.btn = GetComponent<Button>();
        Debug.Log(transform.name + ": LoadButton", gameObject);
    }

    protected override void Start()
    {
        base.Start();
        btn.onClick.AddListener(OnButtonClicked);
    }

    protected virtual void OnButtonClicked()
    {

    }
}
