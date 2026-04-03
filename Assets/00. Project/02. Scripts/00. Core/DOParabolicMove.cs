using System;
using DG.Tweening;
using UnityEngine;

public static class DOParabolicMove
{
    /// <summary>
    /// 움직이는 타겟을 실시간으로 추격하는 안정적인 포물선 이동 (Bezier 기반)
    /// </summary>
    public static Tween MoveToDynamicTarget(
        Transform moveTrs,
        Transform targetTrs,
        float height,
        float duration,
        Vector3 localOffset = default,
        float endRotY = 0f,
        Action onComplete = null)
    {
        Vector3 startPos = moveTrs.position;
        Quaternion startRot = moveTrs.rotation;
        float t = 0f;

        return DOTween.To(() => t, x => t = x, 1f, duration)
            .SetEase(Ease.Linear)
            .SetTarget(moveTrs)
            .OnUpdate(() =>
            {
                if (targetTrs == null || moveTrs == null) return;

                // [Fix]: 로컬 오프셋을 트랜스폼 좌표계에 맞게 변환하여 그리드 적층 및 회전 문제 해결
                Vector3 endPos = targetTrs.TransformPoint(localOffset);
                
                // [Arc Optimization]: 거리에 따른 높이 스케일링 및 최고점 기준점 설정
                float horizontalDist = Vector2.Distance(new Vector2(startPos.x, startPos.z), new Vector2(endPos.x, endPos.z));
                float scaledHeight = height * Mathf.Clamp01(horizontalDist / 2f);
                
                // 2. [Quadratic Bezier]: 시작점, 중간 정점, 끝점을 이용한 안정적 궤적
                Vector3 midPoint = Vector3.Lerp(startPos, endPos, 0.5f);
                midPoint.y = Mathf.Max(startPos.y, endPos.y) + scaledHeight;
                
                // 2차 베지어 공식 적용
                Vector3 m1 = Vector3.Lerp(startPos, midPoint, t);
                Vector3 m2 = Vector3.Lerp(midPoint, endPos, t);
                moveTrs.position = Vector3.Lerp(m1, m2, t);

                // 3. 회전 보간
                Quaternion targetRot = targetTrs.rotation * Quaternion.Euler(0, endRotY, 0);
                moveTrs.rotation = Quaternion.Slerp(startRot, targetRot, t);
            })
            .OnComplete(() =>
            {
                if (targetTrs != null && moveTrs != null)
                {
                    // [Precise Landing]: 최종 위치 강제 동기화
                    moveTrs.position = targetTrs.TransformPoint(localOffset);
                    moveTrs.rotation = targetTrs.rotation * Quaternion.Euler(0, endRotY, 0);
                }

                onComplete?.Invoke();
            });
    }

    /// <summary>
    /// 고정된 좌표로 이동하는 안정적인 포물선 이동 (Bezier 기반 일관성 확보)
    /// </summary>
    public static Tween MoveToStaticPosition(
        Transform moveTrs,
        Vector3 targetPos,
        float height,
        float duration,
        float rotY = 0f,
        Action onComplete = null)
    {
        Vector3 startPos = moveTrs.position;
        float t = 0f;

        // 동적 추적과 동일한 궤적 계산식 적용
        float horizontalDist = Vector2.Distance(new Vector2(startPos.x, startPos.z), new Vector2(targetPos.x, targetPos.z));
        float scaledHeight = height * Mathf.Clamp01(horizontalDist / 2f);
        
        Vector3 midPoint = Vector3.Lerp(startPos, targetPos, 0.5f);
        midPoint.y = Mathf.Max(startPos.y, targetPos.y) + scaledHeight;

        Sequence seq = DOTween.Sequence().SetTarget(moveTrs);

        seq.Join(DOTween.To(() => t, x => t = x, 1f, duration)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                if (moveTrs == null) return;
                Vector3 m1 = Vector3.Lerp(startPos, midPoint, t);
                Vector3 m2 = Vector3.Lerp(midPoint, targetPos, t);
                moveTrs.position = Vector3.Lerp(m1, m2, t);
            }));

        seq.Join(moveTrs.DORotate(new Vector3(0, rotY, 0), duration, RotateMode.FastBeyond360));

        seq.OnComplete(() => 
        {
            if (moveTrs != null) moveTrs.position = targetPos;
            onComplete?.Invoke();
        });

        return seq;
    }
}
