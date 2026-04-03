using UnityEngine;
using Project.Core.Interfaces;
using System.Collections.Generic;

public class NpcWorkerUnlockReward : MonoBehaviour, IUnlockReward
{
    [Header("Reward Settings")]
    [SerializeField] private List<GameObject> workersToActivate;

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
            if (worker != null)
            {
                worker.SetActive(true);
            }
        }
    }
}
