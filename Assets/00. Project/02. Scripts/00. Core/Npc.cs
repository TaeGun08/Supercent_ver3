using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Npc : MonoBehaviour
{
    protected static readonly int IS_WALK = Animator.StringToHash("IsWalk");
    protected Animator animator;
    protected Tween moveTween;

    [Header("Npc Base Settings")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float rotationSpeed = 10f;

    public bool IsMoving { get; private set; }

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    
    public virtual void MoveTo(Vector3 targetPos, System.Action onArrival = null)
    {
        moveTween?.Kill();

        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance < 0.05f)
        {
            IsMoving = false;
            animator.SetBool(IS_WALK, IsMoving);
            onArrival?.Invoke();
            return;
        }

        IsMoving = true;
        animator.SetBool(IS_WALK, IsMoving);

        float duration = distance / moveSpeed;

        Vector3 dir = (targetPos - transform.position).normalized;
        if (dir != Vector3.zero)
        {
            transform.forward = dir; 
        }

        moveTween = transform.DOMove(targetPos, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                IsMoving = false;
                animator.SetBool(IS_WALK, IsMoving);
                onArrival?.Invoke();
            });
    }

    protected virtual void OnDestroy()
    {
        moveTween?.Kill();
    }
}