using System.Collections.Generic;
using UnityEngine;

public class MiningSensor : MonoBehaviour
{
    private HashSet<IMiningTarget> miningTargets = new();
    public HashSet<IMiningTarget> MiningTargets => miningTargets;

    private int objectLayer;
    private Collider sensorCollider;

    private void Awake()
    {
        // [Fail-Fast]: 레이어 설정 검증
        objectLayer = LayerMask.NameToLayer("Object");
        Debug.Assert(objectLayer != -1, "[MiningSensor] 'Object' 레이어가 레이어 설정에 존재하지 않습니다.");
        
        sensorCollider = GetComponent<Collider>();
        Debug.Assert(sensorCollider != null, "[MiningSensor] 센서에 Collider가 부착되어 있지 않습니다.");
        Debug.Assert(sensorCollider.isTrigger, "[MiningSensor] Collider가 Trigger로 설정되어 있지 않습니다.");
    }

    public void Init(float range)
    {
        // 데이터 기반으로 센서 범위 설정 (Sphere/Box 대응)
        if (sensorCollider is SphereCollider sphere)
        {
            sphere.radius = range;
        }
        else if (sensorCollider is BoxCollider box)
        {
            // 전방으로 range만큼 확장하는 구조 예시
            box.size = new Vector3(range * 2f, 2f, range * 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != objectLayer) return;
        
        if (other.TryGetComponent(out IMiningTarget target))
        {
            miningTargets.Add(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != objectLayer) return;

        if (other.TryGetComponent(out IMiningTarget target))
        {
            miningTargets.Remove(target);
        }
    }

    public IMiningTarget GetClosestTarget(Vector3 searchPosition)
    {
        if (miningTargets.Count == 0) return null;

        // null 참조 타겟 정리 (Destroyed objects 등)
        miningTargets.RemoveWhere(t => t == null || t.Transform == null);

        IMiningTarget closestTarget = null;
        float minDistanceSqr = float.MaxValue;

        foreach (var target in miningTargets)
        {
            float sqrDistance = (target.Transform.position - searchPosition).sqrMagnitude;

            if (sqrDistance >= minDistanceSqr) continue;
            minDistanceSqr = sqrDistance;
            closestTarget = target;
        }

        return closestTarget;
    }
}