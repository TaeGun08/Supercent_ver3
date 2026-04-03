using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class MiningController : MonoBehaviour
{
    [Header("Equipments")] [SerializeField]
    private MiningEquipment[] miningEquipments;

    private MiningEquipment currentMiningEq;

    [Header("Stackers")] [SerializeField] private StoneStacker stoneStacker;
    [SerializeField] private PotionStacker potionStacker;

    private PlayerAnimation playerAnimation;
    private float miningTimer;
    private bool isMiningInProgress;
    private bool isInMiningZone;

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();

        currentMiningEq = miningEquipments[0];
        miningTimer = currentMiningEq.MiningData.MiningDelay;
        SetMiningZoneActive(false);
    }

    private void Update()
    {
        if (isInMiningZone == false) return;
        if (potionStacker != null && potionStacker.HasItem) return;
        if (currentMiningEq == null || isMiningInProgress) return;

        UpdateMiningProcess();
    }

    public void SetMiningZoneActive(bool active)
    {
        isInMiningZone = active;

        if (currentMiningEq == null || currentMiningEq.VisualObject == null) return;
        currentMiningEq.VisualObject.SetActive(active);
    }

    private void UpdateMiningProcess()
    {
        MiningSensor sensor = currentMiningEq.MiningSensor;
        MiningDataSO data = currentMiningEq.MiningData;

        if (sensor.MiningTargets.Count <= 0) return;

        miningTimer += Time.deltaTime;

        if (data.Type == MiningType.Burst)
        {
            if (miningTimer >= data.MiningDelay)
            {
                StartCoroutine(PerformBurstMining(sensor));
            }
        }
        else // Continuous (Drill, Bulldozer)
        {
            if (miningTimer >= data.MiningDelay)
            {
                ExecuteMining(sensor);
                miningTimer = 0f;
            }
        }
    }

    private IEnumerator PerformBurstMining(MiningSensor sensor)
    {
        isMiningInProgress = true;
        miningTimer = 0f;

        playerAnimation.PlayMiningAnimation();
        yield return new WaitForSeconds(0.2f);

        ExecuteMining(sensor);

        isMiningInProgress = false;
    }

    private void ExecuteMining(MiningSensor sensor)
    {
        IMiningTarget target = sensor.GetClosestTarget(sensor.transform.position);

        if (target != null)
        {
            IPickupAble resource = target.MineResource(stoneStacker.IsFull);
            sensor.MiningTargets?.Remove(target);

            if (resource != null && !stoneStacker.IsFull)
            {
                DOParabolicMove.MoveToDynamicTarget(
                    resource.Transform,
                    stoneStacker.transform,
                    height: 1.5f,
                    duration: 0.3f,
                    yOffset: stoneStacker.CurrentCount * 0.5f,
                    onComplete: () => { stoneStacker.PushStack(resource); }
                );
            }
        }
    }

    public void ChangeEquipment(int index)
    {
        currentMiningEq.gameObject.SetActive(false);
        currentMiningEq = miningEquipments[index];
        currentMiningEq.gameObject.SetActive(true);


        miningTimer = currentMiningEq.MiningData.MiningDelay;
    }
}