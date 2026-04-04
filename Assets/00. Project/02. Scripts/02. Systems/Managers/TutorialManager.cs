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

    public Transform CurrentTarget => (currentStepIndex < steps.Count) ? steps[currentStepIndex].target : null;

    protected override void Awake()
    {
        base.Awake();
        // [Fix]: Awake에서 즉시 초기화하여 Inventory 등의 조기 이벤트를 놓치지 않음
        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
        if (steps == null || steps.Count == 0) return;

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
            if (navigationArrow != null) navigationArrow.SetTarget(null);
            return;
        }

        TutorialStep step = steps[currentStepIndex];
        
        // 1. 해금 존 활성화
        if (step.unlockZoneToActivate != null)
        {
            step.unlockZoneToActivate.SetActive(true);
        }

        // 2. 카메라 연출
        if (step.needCameraDirecting)
        {
            CameraDirector.Instance.ShowTarget(step.target);
        }

        // 3. 내비게이션 갱신 (마커 및 화살표)
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

    public void TriggerPrisonExpandGuide(Transform prisonUnlockZone, Transform target)
    {
        if (CameraDirector.Instance.IsDirecting) return;
        
        if (prisonUnlockZone != null) prisonUnlockZone.gameObject.SetActive(true);
        CameraDirector.Instance.ShowTarget(target);
        
        if (navigationArrow != null) navigationArrow.SetTarget(target);
    }
}
