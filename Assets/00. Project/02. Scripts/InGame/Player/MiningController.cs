using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MiningController : MonoBehaviour
{
    [Header("Equipments")]
    [SerializeField]private MiningEquipment[] miningEquipments;
    private MiningEquipment currentMiningEq;
    [SerializeField] private StoneStacker stoneStacker;
    [SerializeField] private PotionStacker potionStacker;

    private void Awake()
    {
        MiningContext ctx = new MiningContext()
        {
            PlayerController = GetComponent<PlayerController>(),
            PlayerAnimation = GetComponent<PlayerAnimation>(),
            ItemStacker = stoneStacker,
        };
        
        foreach (MiningEquipment eq in miningEquipments)
        {
            eq.Init(ctx);
        }
        
        currentMiningEq = miningEquipments[0];
    }

    private void Update()
    {
        if (potionStacker.HasItem) return;
        currentMiningEq?.Mining();
    }

    public void SetActiveEquipment(bool active)
    {
        currentMiningEq.Equipment.SetActive(active);
    }

    public void ChangeEquipment(MiningEquipment newMiningEq)
    {
        currentMiningEq.gameObject.SetActive(false);
        currentMiningEq = newMiningEq;
        currentMiningEq.gameObject.SetActive(true);
    }
}
