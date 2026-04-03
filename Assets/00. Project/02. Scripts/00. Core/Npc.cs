using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Npc : MonoBehaviour
{
    protected static readonly int IS_WALK = Animator.StringToHash("IsWalk");
    protected Animator animator;
    protected Tween moveTween;
    protected Rigidbody rb;

    [Header("Npc Base Settings")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float rotationSpeed = 10f;

    public Rigidbody Rb => rb;
    public bool IsMoving { get; private set; }

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    
    public virtual void MoveTo(Vector3 targetPos, System.Action onArrival = null)
    {
        moveTween?.Kill();

        // 이동 시작 시 물리적 관성 제거
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance < 0.1f) // 도착 판정 범위 소폭 확대 (안정성)
        {
            IsMoving = false;
            animator.SetBool(IS_WALK, false);
            onArrival?.Invoke();
            return;
        }

        IsMoving = true;
        animator.SetBool(IS_WALK, true);

        float duration = distance / moveSpeed;

        Vector3 dir = (targetPos - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            transform.forward = dir; 
        }

        moveTween = transform.DOMove(targetPos, duration)
            .SetEase(Ease.Linear)
            .SetUpdate(UpdateType.Fixed) // 물리 업데이트 타이밍과 동기화
            .OnComplete(() =>
            {
                IsMoving = false;
                animator.SetBool(IS_WALK, false);
                
                // 도착 후 즉시 물리 완전 정지
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                
                onArrival?.Invoke();
            });
    }

    protected virtual void OnDestroy()
    {
        moveTween?.Kill();
    }
}