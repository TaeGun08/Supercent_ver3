using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZoneType { Input, Output }

public class CauldronZone : MonoBehaviour
{
    [SerializeField] private ZoneType zoneType;
    [SerializeField] private MagicCauldron cauldron;
    [SerializeField] private float interactionInterval = 0.5f;

    private int playerLayer;
    private Coroutine interactionCoroutine;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        Debug.Assert(cauldron != null, "[CauldronZone] MagicCauldron이 연결되지 않았습니다.");
        Debug.Assert(playerLayer != -1, "[CauldronZone] 'Player' 레이어가 존재하지 않습니다.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        if (interactionCoroutine == null)
        {
            interactionCoroutine = StartCoroutine(InteractionRoutine(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        if (interactionCoroutine != null)
        {
            StopCoroutine(interactionCoroutine);
            interactionCoroutine = null;
        }
    }

    private IEnumerator InteractionRoutine(GameObject playerObj)
    {
        while (true)
        {
            if (zoneType == ZoneType.Input)
            {
                HandleInput(playerObj);
            }
            else
            {
                HandleOutput(playerObj);
            }

            yield return new WaitForSeconds(interactionInterval);
        }
    }

    private void HandleInput(GameObject playerObj)
    {
        // [DIP]: 플레이어의 스톤 스태커 탐색
        StoneStacker stoneStacker = playerObj.GetComponentInChildren<StoneStacker>();
        if (stoneStacker == null || !stoneStacker.HasItem) return;

        // 돌 1개 추출 후 가마솥 투입
        IPickupAble stone = stoneStacker.PopStack();
        if (stone != null)
        {
            // [Visual Feedback]: 가마솥으로 날아가는 연출 (임시: 즉시 투입)
            stone.Release(); 
            cauldron.AddResource();
        }
    }

    private void HandleOutput(GameObject playerObj)
    {
        PotionStacker potionStacker = playerObj.GetComponentInChildren<PotionStacker>();
        if (potionStacker == null || potionStacker.IsFull || !cauldron.HasFinishedPotions) return;

        // 가마솥에서 포션 수령
        Potion potion = cauldron.TakePotion();
        if (potion != null)
        {
            // [Visual Feedback]: 플레이어 스택으로 추가
            potionStacker.PushStack(potion);
        }
    }
}