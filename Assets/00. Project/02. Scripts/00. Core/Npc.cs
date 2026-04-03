using System;
using DG.Tweening;
using UnityEngine;
using Project.Core.Interfaces;

public abstract class Npc : MonoBehaviour
{
    protected static readonly int IS_WALK = Animator.StringToHash("IsWalk");
    
    [Header("Base NPC Settings")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] protected float rotationSpeed = 10f;

    protected Animator animator;
    protected Rigidbody rb;
    protected Tween moveTween;
    protected INpcState currentState;

    public bool IsMoving { get; protected set; }
    public Rigidbody Rb => rb;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        // [State Execution]: 현재 상태의 로직 실행
        currentState?.Execute();
    }

    /// <summary>
    /// 상태 전환 메서드
    /// </summary>
    public void ChangeState(INpcState newState)
    {
        if (newState == null || currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public virtual void MoveTo(Vector3 targetPos, Action onArrival = null)
    {
        moveTween?.Kill();
        IsMoving = true;
        
        if (rb != null) rb.isKinematic = true;
        if (animator != null) animator.SetBool(IS_WALK, true);

        float distance = Vector3.Distance(transform.position, targetPos);
        float duration = distance / moveSpeed;

        transform.DOLookAt(targetPos, 0.2f, AxisConstraint.Y);

        moveTween = transform.DOMove(targetPos, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                IsMoving = false;
                if (animator != null) animator.SetBool(IS_WALK, false);
                if (rb != null)
                {
                    rb.isKinematic = false;
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
