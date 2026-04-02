using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PickAxe : MiningEquipment
{
    [Header("PickAxe")]
    [SerializeField] private GameObject pickAxeObj;
    
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
        pickAxeObj.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        pickAxeObj.SetActive(false);
        isMining = false;
        IMiningTarget stone = miningSensor.GetClosestTarget(miningSensor.transform.position);
        IPickupAble cobblestone = stone?.MineResource(itemStacker.IsFull);
        miningSensor.MiningTargets?.Remove(stone);
        if (itemStacker.IsFull) yield break;
        itemStacker.PushStack(cobblestone);
    }
}
