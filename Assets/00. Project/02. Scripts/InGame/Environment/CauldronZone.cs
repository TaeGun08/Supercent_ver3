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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        interactionCoroutine ??= StartCoroutine(InteractionRoutine(other.gameObject));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        if (interactionCoroutine == null) return;
        StopCoroutine(interactionCoroutine);
        interactionCoroutine = null;
    }

    private IEnumerator InteractionRoutine(GameObject playerObj)
    {
        WaitForSeconds wait = new WaitForSeconds(interactionInterval);
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

            yield return wait;
        }
    }

    private void HandleInput(GameObject playerObj)
    {
        StoneStacker stoneStacker = playerObj.GetComponentInChildren<StoneStacker>();
        if (stoneStacker == null || !stoneStacker.HasItem || !cauldron.CanAcceptMoreStones) return;

        ItemStacker targetStacker = cauldron.GetInputStacker();
        if (targetStacker == null) return;

        IPickupAble stone = stoneStacker.PopStack();
        if (stone == null) return;
        Vector3 targetWorldPos = targetStacker.transform.position + targetStacker.GetNextLocalPosition();

        DOParabolicMove.MoveToStaticPosition(
            stone.Transform, 
            targetWorldPos, 
            height: 1.5f, 
            duration: 0.5f, 
            onComplete: () => {
                targetStacker.PushStack(stone);
            }
        );
    }

    private void HandleOutput(GameObject playerObj)
    {
        PotionStacker potionStacker = playerObj.GetComponentInChildren<PotionStacker>();
        if (potionStacker == null || potionStacker.IsFull || !cauldron.HasFinishedPotions) return;

        Potion potion = cauldron.TakePotion();
        if (potion != null)
        {
            DOParabolicMove.MoveToDynamicTarget(
                potion.Transform, 
                playerObj.transform, 
                height: 2f, 
                duration: 0.5f, 
                yOffset: 1.5f,
                onComplete: () => {
                    potionStacker.PushStack(potion);
                }
            );
        }
    }
}