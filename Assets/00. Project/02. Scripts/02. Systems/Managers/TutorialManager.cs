using System;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialCondition { MineStone, InputStone, TakePotion, DistributePotion, GetGold, UnlockZone }

[Serializable]
public class TutorialStep
{
    public string stepName;
    public TutorialCondition condition;
    public Transform target;
    public bool needCameraDirecting;
    public Transform cameraPoint; // [New]: 카메라가 연출 시 이동할 실제 지점
    public GameObject unlockZoneToActivate;
}

public class TutorialManager : SingletonBase<TutorialManager>
{
    [Header("Tutorial Steps")]
    [SerializeField] private List<TutorialStep> steps = new();
    [SerializeField] private int currentStepIndex = 0;

    [Header("UI & Effect")]
    [SerializeField] private TutorialArrow navigationArrow;

    protected override void Awake()
    {
        base.Awake();
        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
        if (steps == null || steps.Count == 0) return;

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
            if (navigationArrow != null) navigationArrow.SetTarget(null);
            return;
        }

        TutorialStep step = steps[currentStepIndex];
        
        if (step.unlockZoneToActivate != null) step.unlockZoneToActivate.SetActive(true);

        if (step.needCameraDirecting)
        {
            // [Fix]: 카메라 포인트와 타겟을 모두 넘겨줌
            CameraDirector.Instance.ShowTarget(step.target, step.cameraPoint);
        }

        if (navigationArrow != null)
        {
            navigationArrow.SetTarget(step.target);
        }
    }

    public void OnActionPerform(TutorialCondition condition)
    {
        if (currentStepIndex >= steps.Count || CameraDirector.Instance.IsDirecting) return;

        if (steps[currentStepIndex].condition == condition)
        {
            currentStepIndex++;
            RefreshCurrentStep();
        }
    }

    public void TriggerPrisonExpandGuide(Transform prisonUnlockZone, Transform target, Transform cameraPoint = null)
    {
        if (CameraDirector.Instance.IsDirecting) return;
        
        if (prisonUnlockZone != null) prisonUnlockZone.gameObject.SetActive(true);
        CameraDirector.Instance.ShowTarget(target, cameraPoint);
        
        if (navigationArrow != null) navigationArrow.SetTarget(target);
    }
}
