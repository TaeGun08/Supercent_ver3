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
        uiRoot.SetActive(false);
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
        uiRoot.SetActive(true);
    }

    public void Hide()
    {
        uiRoot.SetActive(false);
        targetTransform = null; // 추적 중단
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
        if (targetTransform == null || uiRoot == null) return;

        Vector3 worldPos = targetTransform.position + worldOffset;
        Vector3 viewportPoint = mainCam.WorldToViewportPoint(worldPos);

        bool isInsideScreen = viewportPoint.z > 0 && 
                              viewportPoint.x > 0 && viewportPoint.x < 1 && 
                              viewportPoint.y > 0 && viewportPoint.y < 1;

        uiRoot.SetActive(isInsideScreen);

        if (isInsideScreen)
        {
            transform.position = mainCam.WorldToScreenPoint(worldPos);
        }
    }
}