using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Npc : MonoBehaviour
{
    private static readonly int IS_WALK = Animator.StringToHash("IsWalk");
    protected Animator animator;
    protected Tween moveTween;

    [Header("Npc Base Settings")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float rotationSpeed = 10f;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        Debug.Assert(animator != null, $"[Npc] {gameObject.name}에 Animator가 누락되었습니다.");
    }
    
    public bool IsMoving { get; private set; }

    public virtual void MoveTo(Vector3 targetPos, System.Action onArrival = null)
    {
        moveTween?.Kill();

        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance < 0.05f)
        {
            IsMoving = false;
            onArrival?.Invoke();
            return;
        }

        IsMoving = true;
        float duration = distance / moveSpeed;

        Vector3 dir = (targetPos - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            transform.forward = dir; 
        }

        animator.SetBool(IS_WALK, true);

        moveTween = transform.DOMove(targetPos, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                IsMoving = false;
                animator.SetBool(IS_WALK, false);
                onArrival?.Invoke();
            });
    }

    protected virtual void OnDestroy()
    {
        moveTween?.Kill();
    }
}
