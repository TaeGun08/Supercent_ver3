using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private GameObject arrowVisual;  
    [SerializeField] private GameObject targetMarker;  

    [Header("Marker Subtle Animation")]
    [SerializeField] private float bobbingHeight = 0.2f; 
    [SerializeField] private float bobbingSpeed = 2f;   
    [SerializeField] private float rotationSpeed = 60f; 
    [SerializeField] private float markerYOffset = 2.5f; 

    private Transform target;
    private Camera mainCamera;
    private float animationTimer;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (targetMarker != null) targetMarker.SetActive(false);
        if (arrowVisual != null) arrowVisual.SetActive(false);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        animationTimer = 0f; 

        // [Fix]: 타겟이 null이면 즉시 모든 가이드 오브젝트를 끔
        if (target == null)
        {
            if (arrowVisual != null) arrowVisual.SetActive(false);
            if (targetMarker != null) targetMarker.SetActive(false);
            return;
        }

        if (targetMarker != null)
        {
            targetMarker.SetActive(true);
            UpdateMarkerPosition();
        }
    }

    private void LateUpdate()
    {
        // [Fix]: 타겟이 없으면 로직 수행 안 함
        if (target == null) return;

        // 1. 목적지 마커 실시간 위치 동기화
        if (targetMarker != null && targetMarker.activeSelf)
        {
            UpdateMarkerPosition();
        }

        // 2. 화면 노출 여부 판정
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position);
        bool isVisible = viewportPos.z > 0 && viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;

        // 3. 발밑 화살표 회전 제어
        if (isVisible)
        {
            if (arrowVisual != null && arrowVisual.activeSelf) arrowVisual.SetActive(false);
        }
        else
        {
            if (arrowVisual != null)
            {
                if (!arrowVisual.activeSelf) arrowVisual.SetActive(true);
                UpdateArrowRotation();
            }
        }
    }

    private void UpdateMarkerPosition()
    {
        animationTimer += Time.deltaTime;
        float bobbingY = Mathf.Sin(animationTimer * bobbingSpeed) * bobbingHeight;
        targetMarker.transform.position = target.position + Vector3.up * (markerYOffset + bobbingY);
        targetMarker.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void UpdateArrowRotation()
    {
        Vector3 direction = (target.position - transform.position);
        direction.y = 0; 

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 15f);
        }
    }
}
