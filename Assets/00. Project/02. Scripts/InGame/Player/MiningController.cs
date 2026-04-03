using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MiningController : MonoBehaviour
{
    [Header("Equipments")]
    [SerializeField] private MiningEquipment[] miningEquipments;
    private MiningEquipment currentMiningEq;

    [Header("Stackers")]
    [SerializeField] private StoneStacker stoneStacker;
    [SerializeField] private PotionStacker potionStacker;

    private PlayerAnimation playerAnimation;
    private float miningTimer;
    private bool isMiningInProgress;

    private void Awake()
    {
        // [Fail-Fast]: 필수 컴포넌트 및 장착 장비 검증
        playerAnimation = GetComponent<PlayerAnimation>();
        Debug.Assert(playerAnimation != null, "[MiningController] PlayerAnimation이 누락되었습니다.");
        Debug.Assert(miningEquipments != null && miningEquipments.Length > 0, "[MiningController] 할당된 장비가 없습니다.");
        
        // 장비 센서 초기화 (데이터 동기화)
        foreach (var eq in miningEquipments)
        {
            if (eq != null && eq.MiningSensor != null)
            {
                eq.MiningSensor.Init(eq.MiningData.MiningRange);
            }
        }

        // 초기 장비 설정
        currentMiningEq = miningEquipments[0];
        miningTimer = currentMiningEq.MiningData.MiningDelay;
    }

    private void Update()
    {
        // 포션 스택이 있거나 장비가 없으면 중단
        if (potionStacker != null && potionStacker.HasItem) return;
        if (currentMiningEq == null || isMiningInProgress) return;

        UpdateMiningProcess();
    }

    private void UpdateMiningProcess()
    {
        MiningSensor sensor = currentMiningEq.MiningSensor;
        
        // 주변에 타겟이 없으면 대기
        if (sensor.MiningTargets.Count <= 0) return;

        miningTimer += Time.deltaTime;
        
        if (miningTimer >= currentMiningEq.MiningData.MiningDelay)
        {
            StartCoroutine(PerformMiningAction(sensor));
        }
    }

    private System.Collections.IEnumerator PerformMiningAction(MiningSensor sensor)
    {
        isMiningInProgress = true;
        miningTimer = 0f;

        // 1. 애니메이션 재생
        playerAnimation.PlayMiningAnimation();

        // 2. 애니메이션 타이밍 대기 (데이터화 가능)
        yield return new WaitForSeconds(0.2f);

        // 3. 실제 채굴 수행
        IMiningTarget target = sensor.GetClosestTarget(sensor.transform.position);
        
        if (target != null)
        {
            IPickupAble resource = target.MineResource(stoneStacker.IsFull);
            sensor.MiningTargets?.Remove(target);

            if (resource != null && !stoneStacker.IsFull)
            {
                stoneStacker.PushStack(resource);
            }
        }

        isMiningInProgress = false;
    }

    public void ChangeEquipment(int index)
    {
        Debug.Assert(index >= 0 && index < miningEquipments.Length, "[MiningController] 잘못된 장비 인덱스입니다.");
        
        currentMiningEq.gameObject.SetActive(false);
        currentMiningEq = miningEquipments[index];
        currentMiningEq.gameObject.SetActive(true);
        
        // 센서 범위 재동기화 (장비별 개별 설정 보장)
        if (currentMiningEq.MiningSensor != null)
        {
            currentMiningEq.MiningSensor.Init(currentMiningEq.MiningData.MiningRange);
        }
        
        // 타이머 초기화 (즉시 채굴 가능하도록)
        miningTimer = currentMiningEq.MiningData.MiningDelay;
    }
}
