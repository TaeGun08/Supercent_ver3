using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CauldronState { Idle, Crafting }

public class MagicCauldron : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float craftDuration = 3f;
    [SerializeField] private Potion potionPrefab;
    [SerializeField] private Transform potionSpawnPoint;

    [Header("Current Status")]
    private CauldronState state = CauldronState.Idle;
    private int queuedStones = 0;
    private int producedPotions = 0;

    public bool CanProduceMore => producedPotions < 10; // 임시: 최대 저장량 10개
    public bool HasFinishedPotions => producedPotions > 0;

    private void Awake()
    {
        Debug.Assert(potionPrefab != null, "[MagicCauldron] Potion 프리팹이 할당되지 않았습니다.");
        Debug.Assert(potionSpawnPoint != null, "[MagicCauldron] 포션 스폰 포인트가 할당되지 않았습니다.");
        
        // 포션 풀 생성
        PoolManager.Instance.CreatePool(potionPrefab, initialCount: 5);
    }

    public void AddResource()
    {
        queuedStones++;
        if (state == CauldronState.Idle)
        {
            StartCoroutine(CraftingRoutine());
        }
    }

    public Potion TakePotion()
    {
        if (producedPotions <= 0) return null;
        
        producedPotions--;
        Potion potion = PoolManager.Instance.Get<Potion>();
        potion.transform.SetPositionAndRotation(potionSpawnPoint.position, Quaternion.identity);
        
        return potion;
    }

    private IEnumerator CraftingRoutine()
    {
        while (queuedStones > 0)
        {
            state = CauldronState.Crafting;
            queuedStones--;

            // 제작 시간 대기 (애니메이션 등 추가 가능)
            yield return new WaitForSeconds(craftDuration);

            producedPotions++;
            Debug.Log("[MagicCauldron] 포션 1개 제작 완료! (남은 대기열: " + queuedStones + ")");
        }

        state = CauldronState.Idle;
    }
}
