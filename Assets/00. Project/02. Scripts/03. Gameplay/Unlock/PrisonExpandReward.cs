using UnityEngine;
using Project.Core.Interfaces;

public class PrisonExpandReward : MonoBehaviour, IUnlockReward
{
    [Header("Reward Settings")]
    [SerializeField] private int additionalCapacity = 5;
    [SerializeField] private GameObject[] upgrade;

    public void Execute()
    {
        if (PrisonManager.Instance != null)
        {
            PrisonManager.Instance.ExpandCapacity(additionalCapacity);
        }
    }
}
