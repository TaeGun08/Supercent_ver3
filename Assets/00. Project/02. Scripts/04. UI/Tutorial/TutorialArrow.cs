using UnityEngine;
using DG.Tweening;

public class TutorialArrow : MonoBehaviour
{
    [Header("Visual References")]
    [SerializeField] private GameObject arrowVisual;  
    [SerializeField] private GameObject targetMarker;  

    [Header("Marker Animation (DOTween)")]
    [SerializeField] private float bobbingHeight = 0.4f;
    [SerializeField] private float bobbingDuration = 0.7f;
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float markerYOffset = 3.0f;

    private Transform target;
    private Camera mainCamera;
    private Sequence markerSequence;

    private void Awake()
    {
        mainCamera = Camera.main;
        if (targetMarker != null) targetMarker.SetActive(false);
        if (arrowVisual != null) arrowVisual.SetActive(false);
    }

    public void SetTarget(Transform newTarget)
    {
        // 1. 이전 애니메이션 및 상태 정리
        markerSequence?.Kill();
        target = newTarget;

        if (target == null)
        {
            if (arrowVisual != null) arrowVisual.SetActive(false);
            if (targetMarker != null) targetMarker.SetActive(false);
            return;
        }

        if (targetMarker != null)
        {
            targetMarker.SetActive(true);
            
            // 2. 초기 위치 스냅
            targetMarker.transform.position = target.position + Vector3.up * markerYOffset;
            
            // 3. [DOTween Sequence]: 로컬 좌표를 사용하여 상하 부유 및 회전 구현
            // World 좌표는 LateUpdate에서 타겟을 따라가게 하므로, 애니메이션은 '상대적'이어야 함
            markerSequence = DOTween.Sequence().SetTarget(targetMarker);
            
            // 부드러운 상하 이동 (Yoyo)
            // Tip: transform.DOMoveY 대신 타겟의 머리 위에서 '흔들리는' 연출을 위해 가상 오프셋 활용
            targetMarker.transform.localPosition = target.position + Vector3.up * markerYOffset;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. 목적지 마커 실시간 위치 추적 (세계 좌표 동기화)
        if (targetMarker != null && targetMarker.activeSelf)
        {
            // [Fix]: Update 마다 타겟의 X, Z 좌표를 완벽히 일치시킴
            // Y축은 DOTween 연출을 위해 Sine 곡선이나 가상 오프셋을 사용해야 충돌이 없음
            // 여기서는 DOTween과 충돌을 피하기 위해 마커의 '부모' 역할을 코드로 수행
            
            float bobbingY = Mathf.Sin(Time.time * (1f / bobbingDuration) * 5f) * bobbingHeight;
            targetMarker.transform.position = target.position + Vector3.up * (markerYOffset + bobbingY);
            
            // 회전
            targetMarker.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
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

    private void OnDestroy()
    {
        markerSequence?.Kill();
    }
}
