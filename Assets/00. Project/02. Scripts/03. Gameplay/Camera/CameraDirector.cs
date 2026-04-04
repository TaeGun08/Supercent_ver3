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

    public void ShowTarget(Transform target, Transform specificPoint = null, Action onComplete = null)
    {
        if (isDirecting || target == null) return;
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

        // [New]: 특정 포인트가 있으면 그곳으로, 없으면 타겟 위쪽 자동 계산
        Vector3 viewPos;
        Quaternion viewRot;

        if (specificPoint != null)
        {
            viewPos = specificPoint.position;
            viewRot = specificPoint.rotation;
        }
        else
        {
            viewPos = target.position + new Vector3(0, 7, -4);
            viewRot = Quaternion.LookRotation(target.position - viewPos);
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(viewPos, 1.5f).SetEase(Ease.InOutCubic));
        seq.Join(mainCamera.transform.DORotateQuaternion(viewRot, 1.5f));
        seq.AppendInterval(1.2f);
        seq.Append(mainCamera.transform.DOMove(startPos, 1.2f).SetEase(Ease.InOutSine));
        seq.Join(mainCamera.transform.DORotateQuaternion(startRot, 1.2f));

        yield return seq.WaitForCompletion();

        if (followCamera != null) followCamera.enabled = true;
        if (joystick != null) joystick.enabled = true;

        isDirecting = false;
        onComplete?.Invoke();
    }
}
