using UnityEngine;
using Project.Core.Interfaces;

public class EquipmentUnlockReward : MonoBehaviour, IUnlockReward
{
    [Header("Reward Settings")] [SerializeField]
    private int equipmentIndex;

    [SerializeField] private GameObject[] nextZone;

    public void Execute()
    {
        MiningController miningController = FindFirstObjectByType<MiningController>();
        if (miningController == null) return;
        miningController.ChangeEquipment(equipmentIndex);
        if (nextZone.Length <= 0) return;
        foreach (GameObject zone in nextZone)
        {
            zone?.SetActive(true);
        }
    }
}