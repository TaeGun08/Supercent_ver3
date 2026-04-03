using System;
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

    private RectTransform rectTransform;
    private TMP_Text maxText;
    private Sequence animationSequence;
    private Vector3 originalAnchoredPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        maxText = GetComponent<TMP_Text>();
        
        originalAnchoredPosition = rectTransform.anchoredPosition;
        
        gameObject.SetActive(false);
    }
    
    public void Play()
    {
        animationSequence?.Kill();

        gameObject.SetActive(true);
        rectTransform.anchoredPosition = originalAnchoredPosition;
        
        Color color = maxText.color;
        color.a = 0f;
        maxText.color = color;

        animationSequence = DOTween.Sequence();

        animationSequence
            .Join(rectTransform.DOAnchorPos(originalAnchoredPosition + Vector3.up * moveDistance, fadeDuration).SetEase(Ease.OutCubic))
            .Join(maxText.DOFade(1f, fadeDuration))
            .AppendInterval(displayDuration)
            .Append(maxText.DOFade(0f, fadeDuration))
            .OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        animationSequence?.Kill();
    }
}
