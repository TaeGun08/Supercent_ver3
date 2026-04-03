using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum CauldronState { Idle, Crafting }

public class MagicCauldron : MonoBehaviour
{
    [Header("MagicCauldron Settings")]
    [SerializeField] private ItemStacker putDownStone;
    [SerializeField] private ItemStacker pickUpPotion;
    [SerializeField] private float craftDuration = 0.5f;
    [SerializeField] private Potion potionPrefab;

    [Header("Current Status")]
    private CauldronState state = CauldronState.Idle;

    public bool CanAcceptMoreStones => putDownStone != null && !putDownStone.IsFull;
    public bool HasFinishedPotions => pickUpPotion != null && pickUpPotion.HasItem;

    private void Awake()
    {
        if (potionPrefab == null) return;
        PoolManager.Instance.CreatePool(potionPrefab, initialCount: 5);
    }

    private void Update()
    {
        if (state != CauldronState.Idle || putDownStone.HasItem == false || pickUpPotion.IsFull) return;
        StartCoroutine(CraftingRoutine());
    }

    private IEnumerator CraftingRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(craftDuration);
        while (putDownStone.HasItem && !pickUpPotion.IsFull)
        {
            state = CauldronState.Crafting;

            IPickupAble stone = putDownStone.PopStack();
            if (stone != null) stone.Release();

            yield return wait;

            Potion potion = PoolManager.Instance.Get<Potion>();
            Vector3 targetWorldPos = pickUpPotion.transform.position + pickUpPotion.GetNextLocalPosition();

            DOParabolicMove.MoveToStaticPosition(
                potion.Transform,
                targetWorldPos,
                height: 2f,
                duration: 0.1f,
                onComplete: () => { pickUpPotion.PushStack(potion); }
            );
        }

        state = CauldronState.Idle;
    }

    public ItemStacker GetInputStacker() => putDownStone;
    public ItemStacker GetOutputStacker() => pickUpPotion;
}
