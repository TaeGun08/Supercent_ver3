using UnityEngine;
using TMPro;

public class PrisonerUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject uiRoot;
    [SerializeField] private TextMeshProUGUI potionCountText;
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 2.5f, 0);

    private Transform targetTransform;
    private Camera mainCam;
    private int requiredCount;
    private int currentCount;

    private void Awake()
    {
        mainCam = Camera.main;
        if (uiRoot != null) uiRoot.SetActive(false);
    }

    public void Initialize(Transform target, int total)
    {
        targetTransform = target;
        requiredCount = total;
        currentCount = 0;
        UpdateText();
    }

    public void Show()
    {
        if (uiRoot != null) uiRoot.SetActive(true);
    }

    public void Hide()
    {
        if (uiRoot != null) uiRoot.SetActive(false);
    }

    public void UpdateCount(int count)
    {
        currentCount = count;
        UpdateText();
    }

    private void UpdateText()
    {
        if (potionCountText != null)
        {
            potionCountText.text = $"{currentCount}/{requiredCount}";
        }
    }

    private void LateUpdate()
    {
        if (targetTransform == null || uiRoot == null || !uiRoot.activeSelf) return;

        // 1. 3D 월드 좌표를 스크린 좌표로 변환
        Vector3 worldPos = targetTransform.position + worldOffset;
        Vector3 viewportPoint = mainCam.WorldToViewportPoint(worldPos);

        // 2. 화면 이탈 여부 체크 (최적화)
        bool isInsideScreen = viewportPoint.z > 0 && 
                              viewportPoint.x > 0 && viewportPoint.x < 1 && 
                              viewportPoint.y > 0 && viewportPoint.y < 1;

        // 화면 밖이면 렌더링용 자식 오브젝트만 비활성화 (스크립트 로직은 유지)
        potionCountText.gameObject.SetActive(isInsideScreen);

        if (isInsideScreen)
        {
            transform.position = mainCam.WorldToScreenPoint(worldPos);
        }
    }
}