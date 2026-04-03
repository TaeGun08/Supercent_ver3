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
        StoneStacker stoneStacker = playerObj.GetComponentInChildren<StoneStacker>();
        if (stoneStacker == null || !stoneStacker.HasItem) return;

        IPickupAble stone = stoneStacker.PopStack(playerObj.transform.position);
        if (stone != null)
        {
            // [Visual Feedback]: 가마솥으로 날아가는 연출 후 투입
            DOParabolicMove.MoveToStaticPosition(
                stone.Transform, 
                cauldron.transform.position + Vector3.up * 2f, 
                height: 1.5f, 
                duration: 0.5f, 
                onComplete: () => {
                    stone.Release();
                    cauldron.AddResource();
                }
            );
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
            // [Visual Feedback]: 플레이어 스택으로 날아가는 연출
            DOParabolicMove.MoveToDynamicTarget(
                potion.Transform, 
                playerObj.transform, 
                height: 2f, 
                duration: 0.5f, 
                yOffset: 1.5f, // 등 위쪽 타겟
                onComplete: () => {
                    potionStacker.PushStack(potion);
                }
            );
        }
    }
}