using UnityEngine;
using Project.Core.Interfaces;

public class PrisonExpandReward : MonoBehaviour, IUnlockReward
{
    [Header("Reward Settings")]
    [SerializeField] private int additionalCapacity = 5;
    [SerializeField] private GameObject[] upgrade;

    public void Execute()
    {
        if (PrisonManager.Instance == null) return;
        PrisonManager.Instance.ExpandCapacity(additionalCapacity);
        upgrade[0].gameObject.SetActive(false);
        upgrade[1].gameObject.SetActive(true);
    }
}
