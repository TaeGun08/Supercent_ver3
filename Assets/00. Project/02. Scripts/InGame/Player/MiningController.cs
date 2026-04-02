using System;
using UnityEngine;

public class MiningController : MonoBehaviour
{
    [Header("Equipments")]
    [SerializeField]private MiningEquipment[] miningEquipments;
    private MiningEquipment currentMiningEq;

    private void Awake()
    {
        MiningContext ctx = new MiningContext()
        {
            PlayerController = GetComponent<PlayerController>(),
            PlayerAnimation = GetComponent<PlayerAnimation>(),
            ItemStacker = GetComponent<ItemStacker>(),
        };
        
        foreach (MiningEquipment eq in miningEquipments)
        {
            eq.Init(ctx);
        }
        
        currentMiningEq = miningEquipments[0];
    }

    private void Update()
    {
        currentMiningEq?.Mining();
    }

    public void ChangeEquipment(MiningEquipment newMiningEq)
    {
        currentMiningEq.gameObject.SetActive(false);
        currentMiningEq = newMiningEq;
        currentMiningEq.gameObject.SetActive(true);
    }
}
