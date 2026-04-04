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
            CameraDirector.Instance.ShowTarget(step.target);
        }

        if (navigationArrow != null)
        {
            navigationArrow.SetTarget(step.target);
        }
    }

    public void OnActionPerform(TutorialCondition condition)
    {
        if (currentStepIndex >= steps.Count || CameraDirector.Instance.IsDirecting) return;

        if (steps[currentStepIndex].condition != condition) return;
        currentStepIndex++;
        RefreshCurrentStep();
    }

    public void TriggerPrisonExpandGuide(Transform prisonUnlockZone, Transform target)
    {
        if (CameraDirector.Instance.IsDirecting) return;
        
        if (prisonUnlockZone != null) prisonUnlockZone.gameObject.SetActive(true);
        CameraDirector.Instance.ShowTarget(target);
        
        if (navigationArrow != null) navigationArrow.SetTarget(target);
    }
}
