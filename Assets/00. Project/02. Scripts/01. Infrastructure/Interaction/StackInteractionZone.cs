using System.Collections;
using UnityEngine;

public enum InteractionMode { In, Out }

public class StackInteractionZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private InteractionMode mode;
    [SerializeField] private ItemStacker targetStacker;
    [SerializeField] private float interval = 0.3f;

    private int playerLayer;
    private Coroutine interactionRoutine;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;
        
        interactionRoutine ??= StartCoroutine(ProcessInteraction(other.gameObject));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        if (interactionRoutine == null) return;
        StopCoroutine(interactionRoutine);
        interactionRoutine = null;
    }

    private IEnumerator ProcessInteraction(GameObject playerObj)
    {
        if (playerObj.TryGetComponent(out Inventory stackManager) == false) yield break;
        WaitForSeconds wait = new WaitForSeconds(interval);

        while (true)
        {
            if (mode == InteractionMode.In) HandleInput(stackManager);
            else HandleOutput(stackManager);
            yield return wait;
        }
    }

    private void HandleInput(Inventory inventory)
    {
        ItemStacker playerSource = inventory.GetStacker(targetStacker.AcceptableType);
        
        if (playerSource == null || !playerSource.HasItem || targetStacker.IsFull) return;

        IPickupAble item = playerSource.PopStack();
        if (item == null) return;

        Vector3 targetWorldPos = targetStacker.transform.position + targetStacker.GetNextLocalPosition();
        
        DOParabolicMove.MoveToStaticPosition(
            item.Transform, 
            targetWorldPos, 
            height: 1.5f, 
            duration: 0.1f, 
            onComplete: () => targetStacker.PushStack(item)
        );
    }

    private void HandleOutput(Inventory inventory)
    {
        ItemStacker playerTarget = inventory.GetStacker(targetStacker.AcceptableType);
        
        if (playerTarget == null || playerTarget.IsFull || !targetStacker.HasItem) return;

        IPickupAble item = targetStacker.PopStack();
        if (item == null) return;

        DOParabolicMove.MoveToDynamicTarget(
            item.Transform, 
            playerTarget.transform, 
            height: 2f, 
            duration: 0.1f, 
            yOffset: playerTarget.CurrentCount * playerTarget.ItemHeight,
            onComplete: () => playerTarget.PushStack(item)
        );
    }
}