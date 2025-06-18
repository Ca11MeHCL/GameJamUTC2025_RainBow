using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : ImpBehaviour
{
    private Vector3 originalScale;
    private Tween floatTween;
    private bool isActive = false;

    protected override void Start()
    {
        base.Start();
        this.originalScale = transform.localScale;
        StartFloating();
    }

    protected virtual void StartFloating()
    {
        floatTween = transform.DOMoveY(transform.position.y + 0.3f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void SunActive(Vector3 targetPosition)
    {
        if (isActive) return;
        isActive = true;

        if (floatTween != null && floatTween.IsActive()) floatTween.Kill();

        Sequence sunSequence = DOTween.Sequence();

        sunSequence.Append(transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack)) // Shrink
            .Append(transform.DOMove(targetPosition, 0.4f).SetEase(Ease.InQuad))       // Move to target
            .Append(transform.DOScale(originalScale, 0.3f).SetEase(Ease.OutBack))      // Expand again
            .AppendCallback(() =>
            {
                // Start rotating
                transform.DORotate(new Vector3(0, 0, -360f), 1f, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear);

                // Move right continuously
                transform.DOMoveX(15f, 3f) // Di chuyển đến x = 15f trong 3 giây
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        Destroy(gameObject); // Hủy sau khi tới vị trí
                    });
            });
    }
}
