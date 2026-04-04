using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TutorialCondition { MineStone, InputStone, TakePotion, DistributePotion, GetGold, UnlockZone }

[Serializable]
public class TutorialStep
{
    public string stepName;
    public TutorialCondition condition;
    public Transform target;
    public bool needCameraDirecting;
    public GameObject unlockZoneToActivate;
}

public class TutorialManager : SingletonBase<TutorialManager>
{
    [Header("Tutorial Steps")]
    [SerializeField] private List<TutorialStep> steps = new();
    [SerializeField] private int currentStepIndex = 0;

    [Header("UI & Effect")]
    [SerializeField] private TutorialArrow navigationArrow;
    
    private bool isDirecting;
    private Camera mainCamera;
    private Vector3 cameraOriginalLocalPos;
    private Transform cameraParent;

    public Transform CurrentTarget => (currentStepIndex < steps.Count) ? steps[currentStepIndex].target : null;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        cameraParent = mainCamera.transform.parent; // FollowCamera의 타겟 추적 기능을 유지하기 위함
    }

    private void Start()
    {
        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
        // 모든 튜토리얼용 해금 존은 초기에 꺼둠
        foreach (var step in steps)
        {
            if (step.unlockZoneToActivate != null) step.unlockZoneToActivate.SetActive(false);
        }

        RefreshCurrentStep();
    }

    private void RefreshCurrentStep()
    {
        if (currentStepIndex >= steps.Count)
        {
            navigationArrow.gameObject.SetActive(false);
            return;
        }

        TutorialStep step = steps[currentStepIndex];
        
        // 1. 해금 존 활성화
        if (step.unlockZoneToActivate != null)
        {
            step.unlockZoneToActivate.SetActive(true);
        }

        // 2. 카메라 연출 필요 시 실행
        if (step.needCameraDirecting)
        {
            StartCoroutine(CameraDirectingRoutine(step.target));
        }

        // 3. 내비게이션 갱신
        navigationArrow.SetTarget(step.target);
    }

    public void OnActionPerform(TutorialCondition condition)
    {
        if (currentStepIndex >= steps.Count || isDirecting) return;

        if (steps[currentStepIndex].condition == condition)
        {
            currentStepIndex++;
            RefreshCurrentStep();
        }
    }

    private IEnumerator CameraDirectingRoutine(Transform target)
    {
        isDirecting = true;
        
        // 플레이어 조작 차단 (IInputProvider 비활성화 로직 가정)
        var player = FindObjectOfType<PlayerController>();
        player.enabled = false; 

        // 카메라 이동 (FollowCamera 스크립트를 잠시 끄거나 오프셋 조절)
        var followCam = mainCamera.GetComponentInParent<FollowCamera>();
        if (followCam != null) followCam.enabled = false;

        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        // 타겟 주시 위치 (타겟 상공 10m 지점에서 내려다보기)
        Vector3 viewPos = target.position + new Vector3(0, 10, -5);
        
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCamera.transform.DOMove(viewPos, 1.5f).SetEase(Ease.InOutSine));
        seq.Join(mainCamera.transform.DOLookAt(target.position, 1.5f));
        seq.AppendInterval(1.0f);
        seq.Append(mainCamera.transform.DOMove(startPos, 1.2f).SetEase(Ease.InOutSine));
        seq.Join(mainCamera.transform.DORotateQuaternion(startRot, 1.2f));

        yield return seq.WaitForCompletion();

        if (followCam != null) followCam.enabled = true;
        player.enabled = true;
        isDirecting = false;
    }

    // 감옥 만석 시 외부에서 호출될 특수 트리거
    public void TriggerPrisonExpandGuide(Transform prisonUnlockZone, Transform target)
    {
        if (isDirecting) return;
        prisonUnlockZone.gameObject.SetActive(true);
        StartCoroutine(CameraDirectingRoutine(target));
        navigationArrow.SetTarget(target); // 임시로 타겟 변경
    }
}
