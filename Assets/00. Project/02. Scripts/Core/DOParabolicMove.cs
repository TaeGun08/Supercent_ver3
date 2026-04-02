using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class DOParabolicMove
{
    /// <summary>
    /// 움직이는 타겟을 실시간으로 추적하는 포물선 이동 (동적)
    /// </summary>
    public static Tween MoveToDynamicTarget(
        Transform moveTrs,
        Transform targetTrs,
        float height,
        float duration,
        float yOffset = 0f,
        float endRotY = 0f,
        Action onComplete = null)
    {
        Vector3 startPos = moveTrs.position;
        Quaternion startRot = moveTrs.rotation;
        float t = 0f;

        return DOTween.To(() => t, x => t = x, 1f, duration)
            .SetEase(Ease.OutCubic)
            .SetTarget(moveTrs)
            .OnUpdate(() =>
            {
                if (targetTrs == null) return;

                Vector3 currentTargetPos = targetTrs.position + new Vector3(0, yOffset, 0);

                Vector3 lerpPos = Vector3.Lerp(startPos, currentTargetPos, t);
                lerpPos.y += height * 4f * t * (1f - t);
                moveTrs.position = lerpPos;

                Quaternion targetRot = targetTrs.rotation * Quaternion.Euler(0, endRotY, 0);
                moveTrs.rotation = Quaternion.Lerp(startRot, targetRot, t);
            })
            .OnComplete(() =>
            {
                if (targetTrs != null)
                {
                    moveTrs.position = targetTrs.position + new Vector3(0, yOffset, 0);
                    moveTrs.rotation = targetTrs.rotation * Quaternion.Euler(0, endRotY, 0);
                }

                onComplete?.Invoke();
            });
    }

    /// <summary>
    /// 고정된 월드 좌표로 점프하는 포물선 이동 (정적)
    /// </summary>
    public static Tween MoveToStaticPosition(
        Transform moveTrs,
        Vector3 targetPos,
        float height,
        float duration,
        float rotY = 0f,
        Action onComplete = null)
    {
        Sequence seq = DOTween.Sequence().SetTarget(moveTrs);

        seq.Join(moveTrs.DOJump(targetPos, height, 1, duration).SetEase(Ease.OutCubic));
        seq.Join(moveTrs.DORotate(new Vector3(0, rotY, 0), duration, RotateMode.LocalAxisAdd));

        seq.OnComplete(() => onComplete?.Invoke());

        return seq;
    }
}