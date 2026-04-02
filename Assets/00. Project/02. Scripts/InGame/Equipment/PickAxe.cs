using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PickAxe : MiningEquipment
{
    private bool isMining;

    public override void Mining()
    {
        if (miningSensor.MiningTargets.Count <= 0 || isMining) return;
        
        timer += Time.deltaTime;
        if (timer < miningDelay) return;
        StartCoroutine(MiningCoroutine());
    }

    private IEnumerator MiningCoroutine()
    {
        isMining = true;
        timer = 0f;
        playerAnimation.PlayMiningAnimation();
        yield return new WaitForSeconds(0.2f);
        isMining = false;
        IMiningTarget stone = miningSensor.GetClosestTarget(miningSensor.transform.position);
        IPickupAble cobblestone = stone?.MineResource(itemStacker.IsFull);
        miningSensor.MiningTargets?.Remove(stone);
        if (itemStacker.IsFull) yield break;
        itemStacker.PushStack(cobblestone);
    }
}
