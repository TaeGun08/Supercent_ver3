using System.Collections;
using UnityEngine;

public class MiningController : MonoBehaviour
{
    [Header("Equipments")] [SerializeField]
    private MiningEquipment[] miningEquipments;

    private MiningEquipment currentMiningEq;

    [Header("Inventory")] [SerializeField] private Inventory inventory;

    private PlayerAnimation playerAnimation;
    private float miningTimer;
    private bool isMiningInProgress;
    private bool isInMiningZone;

    private WaitForSeconds wait;

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        wait = new WaitForSeconds(0.2f);
        InitializeDefaultEquipment();
    }

    private void InitializeDefaultEquipment()
    {
        currentMiningEq = miningEquipments[0];
        miningTimer = currentMiningEq.MiningData.MiningDelay;
        SetMiningZoneActive(false);
    }

    private void Update()
    {
        if (CanProcessMining() == false) return;

        UpdateMiningCycle();
    }

    private bool CanProcessMining()
    {
        if (!isInMiningZone) return false;
        if (inventory.PotionStacker != null && inventory.PotionStacker.HasItem) return false;
        return currentMiningEq != null && !isMiningInProgress;
    }

    private void UpdateMiningCycle()
    {
        MiningSensor sensor = currentMiningEq.MiningSensor;
        MiningDataSO data = currentMiningEq.MiningData;

        if (sensor.MiningTargets.Count <= 0) return;

        miningTimer += Time.deltaTime;

        if (data.Type == MiningType.Burst)
        {
            if (miningTimer >= data.MiningDelay)
            {
                StartCoroutine(PerformBurstMiningRoutine(sensor));
            }
        }
        else
        {
            if (miningTimer < data.MiningDelay) return;
            ExecuteMiningLogic(sensor);
            miningTimer = 0f;
        }
    }

    private IEnumerator PerformBurstMiningRoutine(MiningSensor sensor)
    {
        isMiningInProgress = true;
        miningTimer = 0f;

        playerAnimation.PlayMiningAnimation();
        yield return wait;

        ExecuteMiningLogic(sensor);

        isMiningInProgress = false;
    }

    private void ExecuteMiningLogic(MiningSensor sensor)
    {
        IMiningTarget target = sensor.GetClosestTarget(sensor.transform.position);
        if (target == null) return;

        ItemStacker stoneStacker = inventory.StoneStacker;
        
        if (stoneStacker.IsFull)
        {
            UIManager.Instance.ShowMaxUI(transform);
            return;
        }

        IPickupAble resource = target.MineResource(false);
        sensor.MiningTargets?.Remove(target);

        if (resource != null)
        {
            PlayMiningVisualEffect(resource, stoneStacker);
        }
    }

    private void PlayMiningVisualEffect(IPickupAble resource, ItemStacker targetStacker)
    {
        DOParabolicMove.MoveToDynamicTarget(
            resource.Transform,
            targetStacker.transform,
            height: 0f,
            duration: 0.1f,
            yOffset: targetStacker.CurrentCount * targetStacker.ItemHeight,
            onComplete: () => { targetStacker.PushStack(resource); }
        );
    }

    public void SetMiningZoneActive(bool active)
    {
        isInMiningZone = active;

        if (currentMiningEq != null && currentMiningEq.VisualObject != null)
        {
            currentMiningEq.VisualObject.SetActive(active);
        }
    }

    public void ChangeEquipment(int index)
    {
        if (currentMiningEq != null) currentMiningEq.gameObject.SetActive(false);
        currentMiningEq = miningEquipments[index];
        currentMiningEq.gameObject.SetActive(true);

        miningTimer = currentMiningEq.MiningData.MiningDelay;
    }
}