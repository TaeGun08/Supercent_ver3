using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    private Transform target;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        gameObject.SetActive(target != null);
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. 타겟이 화면 안에 있는지 확인 (Viewport 좌표 이용)
        Vector3 screenPos = mainCamera.WorldToViewportPoint(target.position);
        bool isVisible = screenPos.x > 0 && screenPos.x < 1 && screenPos.y > 0 && screenPos.y < 1 && screenPos.z > 0;

        // 2. 화면 밖에 있을 때만 화살표를 시각화 (선택 사항: 기획에 따라 항상 띄울 수도 있음)
        // 여기서는 화살표 오브젝트 자체의 활성화 여부를 조절하거나 투명도를 조절할 수 있습니다.
        // 요구사항에 따라 '회전을 통해 진행 위치를 forward로 바라봄'에 집중합니다.

        Vector3 direction = (target.position - transform.position);
        direction.y = 0; // 수평 회전만 유지

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }
}
