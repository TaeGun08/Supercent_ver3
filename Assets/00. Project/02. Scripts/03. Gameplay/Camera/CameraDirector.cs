using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class CameraDirector : SingletonBase<CameraDirector>
{
    private Camera mainCam;
    private bool isDirecting;

    public bool IsDirecting => isDirecting;

    protected override void Awake()
    {
        base.Awake();
        mainCam = GetComponent<Camera>();
    }
    public void ShowTarget(Transform target, Action onComplete = null)
    {
        if (isDirecting || target == null) return;
        StartCoroutine(DirectingRoutine(target, onComplete));
    }

    private IEnumerator DirectingRoutine(Transform target, Action onComplete)
    {
        isDirecting = true;

        Joystick joystick = FindObjectOfType<Joystick>();
        if (joystick != null)
        {
            joystick.EndStick();
            joystick.enabled = false;
        }

        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;

        Vector3 viewPos = target.position + new Vector3(0, 10, -5);

        Sequence seq = DOTween.Sequence();
        seq.Append(mainCam.transform.DOMove(viewPos, 1.5f).SetEase(Ease.InOutCubic));
        seq.Join(mainCam.transform.DOLookAt(target.position, 1.5f));
        seq.AppendInterval(1.0f);
        seq.Append(mainCam.transform.DOMove(startPos, 1.2f).SetEase(Ease.InOutSine));
        seq.Join(mainCam.transform.DORotateQuaternion(startRot, 1.2f));

        yield return seq.WaitForCompletion();

        if (joystick != null) joystick.enabled = true;

        isDirecting = false;
        onComplete?.Invoke();
    }
}
