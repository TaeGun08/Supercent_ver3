using UnityEngine;

public class BullDozer : MiningEquipment
{
    public override void Mining()
    {
        if (miningSensor.MiningTargets.Count <= 0) return;
        IMiningTarget stone = miningSensor.GetClosestTarget(miningSensor.transform.position);
        IPickupAble cobblestone = stone.MineResource(itemStacker.IsFull);
        miningSensor.MiningTargets.Remove(stone);
        if (itemStacker.IsFull) return;
        itemStacker.PushStack(cobblestone);
    }
}
