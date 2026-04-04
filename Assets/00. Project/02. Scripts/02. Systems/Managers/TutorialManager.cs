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
    private Camera mainCam;
    private Vector3 cameraOriginalLocalPos;
    private Transform cameraParent;

    public Transform CurrentTarget => (currentStepIndex < steps.Count) ? steps[currentStepIndex].target : null;

    protected override void Awake()
    {
        base.Awake();
        mainCam = Camera.main;
        cameraParent = mainCam.transform.parent;
    }

    private void Start()
    {
        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
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
        
        if (step.unlockZoneToActivate != null)
        {
            step.unlockZoneToActivate.SetActive(true);
        }

        if (step.needCameraDirecting)
        {
            StartCoroutine(CameraDirectingRoutine(step.target));
        }

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
        
        var player = FindObjectOfType<PlayerController>();
        player.enabled = false; 

        var followCam = mainCam.GetComponentInParent<FollowCamera>();
        if (followCam != null) followCam.enabled = false;

        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;

        Vector3 viewPos = target.position + new Vector3(0, 10, -5);
        
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCam.transform.DOMove(viewPos, 1.5f).SetEase(Ease.InOutSine));
        seq.Join(mainCam.transform.DOLookAt(target.position, 1.5f));
        seq.AppendInterval(1.0f);
        seq.Append(mainCam.transform.DOMove(startPos, 1.2f).SetEase(Ease.InOutSine));
        seq.Join(mainCam.transform.DORotateQuaternion(startRot, 1.2f));

        yield return seq.WaitForCompletion();

        if (followCam != null) followCam.enabled = true;
        player.enabled = true;
        isDirecting = false;
    }

    public void TriggerPrisonExpandGuide(Transform prisonUnlockZone, Transform target)
    {
        if (isDirecting) return;
        prisonUnlockZone.gameObject.SetActive(true);
        StartCoroutine(CameraDirectingRoutine(target));
        navigationArrow.SetTarget(target); 
    }
}
