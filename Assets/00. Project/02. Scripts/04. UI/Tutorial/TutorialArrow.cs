using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private GameObject arrowVisual;  // 플레이어 발밑 3D 모델
    [SerializeField] private GameObject targetMarker;  // 목적지 위에 띄울 3D 모델

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Vector3 markerOffset = new Vector3(0, 3f, 0);

    private Transform target;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (arrowVisual != null) arrowVisual.SetActive(false);
        if (targetMarker != null) targetMarker.SetActive(false);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        
        if (target == null)
        {
            if (arrowVisual != null) arrowVisual.SetActive(false);
            if (targetMarker != null) targetMarker.SetActive(false);
            return;
        }

        if (targetMarker == null) return;
        targetMarker.SetActive(true);
        targetMarker.transform.position = target.position + markerOffset;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (targetMarker != null)
        {
            if (!targetMarker.activeSelf) targetMarker.SetActive(true);
            targetMarker.transform.position = target.position + markerOffset;
        }

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position);
        
        bool isInFront = viewportPos.z > 0;
        bool isInScreen = viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;
        bool isVisible = isInFront && isInScreen;

        if (isVisible)
        {
            if (arrowVisual != null && arrowVisual.activeSelf) arrowVisual.SetActive(false);
        }
        else
        {
            if (arrowVisual == null) return;
            if (!arrowVisual.activeSelf) arrowVisual.SetActive(true);
            UpdateArrowRotation();
        }
    }

    private void UpdateArrowRotation()
    {
        Vector3 direction = (target.position - transform.position);
        direction.y = 0;

        transform.forward = direction;
    }
}
