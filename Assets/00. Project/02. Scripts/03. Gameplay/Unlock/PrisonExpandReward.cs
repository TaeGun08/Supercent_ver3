using UnityEngine;
using Project.Core.Interfaces;

public class PrisonExpandReward : MonoBehaviour, IUnlockReward
{
    [Header("Reward Settings")]
    [SerializeField] private int additionalCapacity = 5;
    [SerializeField] private GameObject[] upgrade;

    [Header("Camera Settings")]
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Transform cameraPoint;

    public void Execute()
    {
        if (PrisonManager.Instance == null) return;
        
        // [Camera Polish]: 해금 시 카메라 연출 트리거
        if (CameraDirector.Instance != null && (lookTarget != null || cameraPoint != null))
        {
            CameraDirector.Instance.ShowTarget(lookTarget, cameraPoint);
        }

        PrisonManager.Instance.ExpandCapacity(additionalCapacity);
        upgrade[0].gameObject.SetActive(false);
        upgrade[1].gameObject.SetActive(true);
    }
}
