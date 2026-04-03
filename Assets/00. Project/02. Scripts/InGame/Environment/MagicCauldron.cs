using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CauldronState { Idle, Crafting }

public class MagicCauldron : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private ItemStacker inputStack;
    [SerializeField] private float craftDuration = 3f;
    [SerializeField] private Potion potionPrefab;
    [SerializeField] private Transform potionSpawnPoint;

    [Header("Current Status")]
    private CauldronState state = CauldronState.Idle;
    private int producedPotions = 0;

    public bool CanAcceptMoreStones => inputStack != null && !inputStack.IsFull;
    public bool HasFinishedPotions => producedPotions > 0;

    private void Awake()
    {
        PoolManager.Instance.CreatePool(potionPrefab, initialCount: 5);
    }

    private void Update()
    {
        if (state == CauldronState.Idle && inputStack.HasItem)
        {
            StartCoroutine(CraftingRoutine());
        }
    }

    private IEnumerator CraftingRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(craftDuration);
        while (inputStack.HasItem)
        {
            state = CauldronState.Crafting;

            IPickupAble stone = inputStack.PopStack();
            if (stone != null) stone.Release();

            yield return wait;

            producedPotions++;
        }

        state = CauldronState.Idle;
    }

    public Potion TakePotion()
    {
        if (producedPotions <= 0) return null;
        producedPotions--;
        
        Potion potion = PoolManager.Instance.Get<Potion>();
        potion.transform.SetPositionAndRotation(potionSpawnPoint.position, Quaternion.identity);
        return potion;
    }

    public ItemStacker GetInputStacker() => inputStack;
}
