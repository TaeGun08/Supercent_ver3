using System.Collections;
using UnityEngine;

public class MiningController : MonoBehaviour
{
    [Header("Equipments")] 
    [SerializeField] private MiningEquipment[] miningEquipments;

    private MiningEquipment currentMiningEq;
    private Inventory inventory;

    private PlayerAnimation playerAnimation;
    private float miningTimer;
    private bool isMiningInProgress;
    private bool isInMiningZone;

    private WaitForSeconds wait;

    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        inventory = GetComponent<Inventory>();
        
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
        bool isFull = stoneStacker.IsFullWithPending; // 비행 중인 물량 포함 체크

        if (isFull)
        {
            UIManager.Instance.ShowMaxUI(transform);
        }

        // 이펙트 재생 (타겟 위치)
        EffectManager.Instance.PlayEffect("MiningHit", target.Transform.position);

        // 슬롯 예약 (리소스 생성 성공 여부와 상관없이 공간 선점 시도)
        if (!isFull) stoneStacker.ReserveSlot();

        IPickupAble resource = target.MineResource(isFull);
        sensor.MiningTargets?.Remove(target);

        if (resource != null)
        {
            PlayMiningVisualEffect(resource, stoneStacker);
        }
        else if (!isFull)
        {
            // 리소스 생성이 취소된 경우(파괴만 된 경우) 예약 취소
            stoneStacker.CancelReservation();
        }
    }

    private void PlayMiningVisualEffect(IPickupAble resource, ItemStacker targetStacker)
    {
        DOParabolicMove.MoveToDynamicTarget(
            resource.Transform,
            targetStacker.transform,
            height: 0f,
            duration: 0.1f,
            localOffset: targetStacker.GetNextLocalPosition(),
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

        inventory.StoneStacker.LimitMaxStackCount(8 * index);
        miningTimer = currentMiningEq.MiningData.MiningDelay;
    }
}