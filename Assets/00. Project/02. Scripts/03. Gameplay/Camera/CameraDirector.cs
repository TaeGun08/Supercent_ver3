using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Camera))]
public class CameraDirector : SingletonBase<CameraDirector>
{
    private Camera mainCamera;
    private FollowCamera followCamera;
    private bool isDirecting;

    public bool IsDirecting => isDirecting;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = GetComponent<Camera>();
        followCamera = GetComponentInParent<FollowCamera>();
    }

    /// <summary>
    /// 지정된 포인트나 타겟을 비춥니다. 타겟이 없어도 포인트만 있다면 작동합니다.
    /// </summary>
    public void ShowTarget(Transform target, Transform specificPoint = null, Action onComplete = null)
    {
        // 타겟과 포인트 둘 다 없으면 연출 불가
        if (isDirecting || (target == null && specificPoint == null)) return;
        
        StartCoroutine(DirectingRoutine(target, specificPoint, onComplete));
    }

    private IEnumerator DirectingRoutine(Transform target, Transform specificPoint, Action onComplete)
    {
        isDirecting = true;

        Joystick joystick = FindObjectOfType<Joystick>();
        if (joystick != null)
        {
            joystick.EndStick();
            joystick.enabled = false;
        }

        if (followCamera != null) followCamera.enabled = false;

        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        // [연출 위치 결정]
        Vector3 viewPos;
        if (specificPoint != null)
        {
            viewPos = specificPoint.position;
        }
        else
        {
            // 포인트가 없을 때만 타겟 기준 자동 좌표 계산
            viewPos = target.position + new Vector3(0, 8, -6);
        }

        Sequence seq = DOTween.Sequence();
        
        // 1. 포지션만 이동 (회전은 현재 각도 유지)
        seq.Append(mainCamera.transform.DOMove(viewPos, 1.2f).SetEase(Ease.OutCubic));
        
        seq.AppendInterval(1.2f); // 머무르는 시간

        // 2. 복귀
        seq.Append(mainCamera.transform.DOMove(startPos, 1.0f).SetEase(Ease.InSine));

        yield return seq.WaitForCompletion();

        if (followCamera != null) followCamera.enabled = true;
        if (joystick != null) joystick.enabled = true;

        isDirecting = false;
        onComplete?.Invoke();
    }
}
