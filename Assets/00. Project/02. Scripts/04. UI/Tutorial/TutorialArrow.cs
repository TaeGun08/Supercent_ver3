using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [Header("Visual References")] 
    [SerializeField] private GameObject arrowVisual;

    [SerializeField] private GameObject targetMarker;

    [Header("Settings")] [SerializeField] private float rotationSpeed = 10f;
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
            targetMarker.transform.position = target.position + markerOffset;
        }

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position);
        bool isVisible = viewportPos.x is > 0 and < 1 && viewportPos.y is > 0 and < 1 && viewportPos.z > 0;

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

        if (direction.magnitude <= 0.1f) return;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
    }
}