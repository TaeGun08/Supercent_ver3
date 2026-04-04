using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private GameObject arrowVisual;
    [SerializeField] private GameObject targetMarker;

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

        // [Fix]: 타겟이 설정되는 즉시 마커를 켜고 위치 이동
        if (targetMarker != null)
        {
            targetMarker.SetActive(true);
            targetMarker.transform.position = target.position + markerOffset;
        }
    }

    private void LateUpdate()
    {
        // 타겟이 없으면 마커도 끔 (방어적 코드)
        if (target == null)
        {
            if (targetMarker != null && targetMarker.activeSelf) targetMarker.SetActive(false);
            if (arrowVisual != null && arrowVisual.activeSelf) arrowVisual.SetActive(false);
            return;
        }

        // 1. 마커 위치 실시간 동기화
        if (targetMarker != null)
        {
            // 타겟의 부모가 꺼져있을 가능성 체크 (안전장치)
            if (target.gameObject.activeInHierarchy)
            {
                if (!targetMarker.activeSelf) targetMarker.SetActive(true);
                targetMarker.transform.position = target.position + markerOffset;
            }
            else
            {
                if (targetMarker.activeSelf) targetMarker.SetActive(false);
            }
        }

        // 2. 화면 노출 여부 판정
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position);
        bool isVisible = viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0;

        // 3. 발밑 화살표 제어
        if (isVisible)
        {
            if (arrowVisual != null && arrowVisual.activeSelf) arrowVisual.SetActive(false);
        }
        else
        {
            // 타겟이 화면 밖일 때만 화살표 활성화
            if (arrowVisual != null && target.gameObject.activeInHierarchy)
            {
                if (!arrowVisual.activeSelf) arrowVisual.SetActive(true);
                UpdateArrowRotation();
            }
        }
    }

    private void UpdateArrowRotation()
    {
        Vector3 direction = (target.position - transform.position);
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }
}
