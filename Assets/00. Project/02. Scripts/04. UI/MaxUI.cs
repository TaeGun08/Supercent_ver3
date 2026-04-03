using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MaxUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveDistance = 50f;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float displayDuration = 1.0f;
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 2.5f, 0);

    private RectTransform rectTransform;
    private TMP_Text maxText;
    private Sequence animationSequence;
    
    private Transform targetTransform;
    private Camera mainCam;
    private float currentVisualOffset;
    private bool isShowing;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        maxText = GetComponent<TMP_Text>();
        mainCam = Camera.main;
        
        gameObject.SetActive(false);
    }
    
    public void Play(Transform target)
    {
        // [State Guard]: 이미 재생 중인 경우 중복 실행 방지
        if (isShowing) return;
        isShowing = true;

        targetTransform = target;
        currentVisualOffset = 0f;
        animationSequence?.Kill();

        gameObject.SetActive(true);
        
        Color color = maxText.color;
        color.a = 0f;
        maxText.color = color;

        UpdatePosition();

        animationSequence = DOTween.Sequence();

        animationSequence
            .Join(DOTween.To(() => currentVisualOffset, x => currentVisualOffset = x, moveDistance, fadeDuration + displayDuration).SetEase(Ease.OutCubic))
            .Join(maxText.DOFade(1f, fadeDuration))
            .AppendInterval(displayDuration)
            .Append(maxText.DOFade(0f, fadeDuration))
            .OnComplete(() => 
            {
                isShowing = false;
                gameObject.SetActive(false);
            });
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (targetTransform == null || mainCam == null || !gameObject.activeSelf) return;

        Vector3 worldPos = targetTransform.position + worldOffset;
        Vector3 viewportPoint = mainCam.WorldToViewportPoint(worldPos);

        bool isInsideScreen = viewportPoint.z > 0 && 
                              viewportPoint.x > 0 && viewportPoint.x < 1 && 
                              viewportPoint.y > 0 && viewportPoint.y < 1;

        maxText.gameObject.SetActive(isInsideScreen);

        if (isInsideScreen)
        {
            // 타겟의 스크린 위치 + 애니메이션 오프셋 적용
            Vector3 screenPos = mainCam.WorldToScreenPoint(worldPos);
            screenPos.y += currentVisualOffset; 
            transform.position = screenPos;
        }
    }

    private void OnDisable()
    {
        animationSequence?.Kill();
        isShowing = false;
        targetTransform = null;
    }
}
