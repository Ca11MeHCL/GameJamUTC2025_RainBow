using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBtn : UIButton
{
    [Header("SpawnBtn")]
    [SerializeField] protected ColorSpawnerRandom colorSpawnerRandom;

    //private string _currentAnimState;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadColorSpawnerRandom();
    }

    protected virtual void LoadColorSpawnerRandom()
    {
        if (this.colorSpawnerRandom != null) return;
        this.colorSpawnerRandom = FindObjectOfType<ColorSpawnerRandom>();
        Debug.Log(transform.name + ": LoadColorSpawnerRandom", gameObject);
    }

    protected override void OnButtonClicked()
    {
        base.OnButtonClicked();
        colorSpawnerRandom.ColorSpawnRandom();
    }

    //public virtual void ChangeAnimationState(string newState)
    //{
    //    // Stop the same animation from interrupting itself
    //    if (_currentAnimState == newState) return;

    //    // Play the animation
    //    _charCtrl.Animator.Play(newState);

    //    // Reassign the current state
    //    _currentAnimState = newState;
    //}
}
