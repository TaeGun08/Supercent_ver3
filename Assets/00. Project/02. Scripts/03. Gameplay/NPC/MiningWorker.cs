using System.Collections;
using UnityEngine;

public enum MiningWorkerState { MovingToStart, Scanning, Mining, MovingToNext }

public class MiningWorker : Npc
{
    private static readonly int MINING = Animator.StringToHash("Mining");

    [Header("Mining Lane")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    
    [Header("Mining Settings")]
    [SerializeField] private ItemStacker targetStorage;
    [SerializeField] private float scanDistance = 1.5f;
    [SerializeField] private float miningDelay = 1.0f;
    [SerializeField] private LayerMask targetLayer;

    private MiningWorkerState currentState;
    private WaitForSeconds miningWait;
    private Vector3 moveDirection;

    protected override void Awake()
    {
        base.Awake();
        
        miningWait = new WaitForSeconds(miningDelay);
        moveDirection = (endPoint.position - startPoint.position).normalized;
    }

    private void Start()
    {
        if (targetStorage == null) return;
        StartCoroutine(WorkerRoutine());
    }

    private IEnumerator WorkerRoutine()
    {
        while (true)
        {
            currentState = MiningWorkerState.MovingToStart;
            MoveTo(startPoint.position);
            yield return new WaitUntil(() => !IsMoving);

            currentState = MiningWorkerState.Scanning;
            while (Vector3.Distance(transform.position, endPoint.position) > 0.5f)
            {
                IMiningTarget target = ScanForTarget();
                
                if (target != null)
                {
                    currentState = MiningWorkerState.Mining;
                    yield return StartCoroutine(PerformMining(target));
                    currentState = MiningWorkerState.Scanning;
                }
                else
                {
                    currentState = MiningWorkerState.MovingToNext;
                    Vector3 nextPos = transform.position + moveDirection * 1.0f;
                    
                    if (Vector3.Distance(startPoint.position, nextPos) > Vector3.Distance(startPoint.position, endPoint.position))
                    {
                        nextPos = endPoint.position;
                    }
                    
                    MoveTo(nextPos);
                    yield return new WaitUntil(() => !IsMoving);
                }
                
                yield return null;
            }
        }
    }

    private IMiningTarget ScanForTarget()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out RaycastHit hit, scanDistance, targetLayer))
        {
            if (hit.collider.TryGetComponent(out IMiningTarget target))
            {
                return target;
            }
        }
        return null;
    }

    private IEnumerator PerformMining(IMiningTarget target)
    {
        animator.SetTrigger(MINING);
        yield return miningWait;

        IPickupAble resource = target.MineResource(targetStorage.IsFull);
        
        if (resource != null)
        {
            Vector3 targetWorldPos = targetStorage.transform.position + targetStorage.GetNextLocalPosition();
            
            DOParabolicMove.MoveToStaticPosition(
                resource.Transform,
                targetWorldPos,
                height: 3f,
                duration: 0.5f,
                onComplete: () => 
                {
                    targetStorage.PushStack(resource);
                }
            );
            
            Debug.Log($"[MiningWorker] 자원을 보관소({targetStorage.name})로 운송했습니다.");
        }
        
        yield return null;
    }
}
