using System.Collections.Generic;
using UnityEngine;

public class MiningSensor : MonoBehaviour
{
    private HashSet<IMiningTarget> miningTargets = new();
    public HashSet<IMiningTarget> MiningTargets => miningTargets;

    private int objectLayer;

    private void Awake()
    {
        objectLayer = LayerMask.NameToLayer("Object");
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