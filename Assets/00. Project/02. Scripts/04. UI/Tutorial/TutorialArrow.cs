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

        // 새 타겟이 들어오면 마커 즉시 이동 및 활성화
        if (targetMarker != null)
        {
            targetMarker.SetActive(true);
            targetMarker.transform.position = target.position + markerOffset;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. 마커 위치 실시간 동기화
        if (targetMarker != null)
        {
            // 타겟 자체가 꺼져있어도 마커는 보여야 함 (해금 전 안내 등)
            // 따라서 target.gameObject.activeInHierarchy 체크를 제거하거나 완화
            if (!targetMarker.activeSelf) targetMarker.SetActive(true);
            targetMarker.transform.position = target.position + markerOffset;
        }

        // 2. 화면 노출 여부 판정 (Viewport 기준)
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position);
        
        // z가 0보다 커야 카메라 앞쪽임
        bool isInFront = viewportPos.z > 0;
        bool isInScreen = viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;
        bool isVisible = isInFront && isInScreen;

        // 3. 발밑 화살표 제어
        if (isVisible)
        {
            // 화면에 목적지가 보이면 발밑 화살표는 숨김
            if (arrowVisual != null && arrowVisual.activeSelf) arrowVisual.SetActive(false);
        }
        else
        {
            // 화면 밖에 목적지가 있으면 발밑 화살표 활성화
            if (arrowVisual != null)
            {
                if (!arrowVisual.activeSelf) arrowVisual.SetActive(true);
                UpdateArrowRotation();
            }
        }
    }

    private void UpdateArrowRotation()
    {
        // 내(화살표) 위치에서 타겟 위치로의 방향 계산
        Vector3 direction = (target.position - transform.position);
        direction.y = 0; // 수평 유지

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }
}
