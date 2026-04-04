using UnityEngine;
using Project.Core.Interfaces;
using System.Collections.Generic;

public class NpcWorkerUnlockReward : MonoBehaviour, IUnlockReward
{
    [Header("Reward Settings")]
    [SerializeField] private List<GameObject> workersToActivate;
    [SerializeField] private GameObject nextUnlock;
    
    private void Awake()
    {
        if (workersToActivate != null)
        {
            foreach (var worker in workersToActivate)
            {
                if (worker != null) worker.SetActive(false);
            }
        }
    }

    public void Execute()
    {
        if (workersToActivate == null) return;
        foreach (var worker in workersToActivate)
        {
            if (worker == null) continue;
            worker.SetActive(true);
            EffectManager.Instance.PlayEffect("WorkerUnlock", worker.transform.position);
        }
        nextUnlock?.gameObject.SetActive(true);
    }
}
