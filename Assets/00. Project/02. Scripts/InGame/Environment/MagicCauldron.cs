using System.Collections;
using UnityEngine;

public enum CauldronState { Idle, Crafting }

public class MagicCauldron : MonoBehaviour
{
    private static readonly int CRAFT = Animator.StringToHash("Craft");
    private Animator animator;
    
    [Header("Dependencies")] 
    [SerializeField] private ItemStacker inputStack;
    [SerializeField] private ItemStacker outputStack;
    [SerializeField] private Potion potionPrefab;

    [Header("Settings")]
    [SerializeField] private float craftDuration = 3f;
    private float moveDuration = 0.3f;

    [Header("Current Status")] 
    private CauldronState state = CauldronState.Idle;

    private WaitForSeconds craftWait;
    private WaitForSeconds moveWait;

    public bool CanAcceptMoreStones => inputStack != null && !inputStack.IsFull;
    public bool HasFinishedPotions => outputStack != null && outputStack.HasItem;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        
        PoolManager.Instance.CreatePool(potionPrefab, initialCount: 5);

        craftWait = new WaitForSeconds(craftDuration);
        moveWait = new WaitForSeconds(moveDuration);
    }

    private void Update()
    {
        if (state != CauldronState.Idle || inputStack.HasItem == false || outputStack.IsFull) return;
        StartCoroutine(CraftingRoutine());
    }

    private IEnumerator CraftingRoutine()
    {
        state = CauldronState.Crafting;

        while (inputStack.HasItem && outputStack.IsFull == false)
        {
            IPickupAble stone = inputStack.PopStack();
            if (stone != null)
            {
                DOParabolicMove.MoveToStaticPosition(
                    stone.Transform,
                    transform.position,
                    height: 2f,
                    duration: moveDuration,
                    onComplete: () => { stone.Release(); }
                );
                
                yield return moveWait; 
            }

            animator.SetTrigger(CRAFT);
            yield return craftWait;

            Potion potion = PoolManager.Instance.Get<Potion>();
            if (potion == null) break;

            potion.Transform.position = transform.position;
            Vector3 targetWorldPos = outputStack.transform.position + outputStack.GetNextLocalPosition();

            DOParabolicMove.MoveToStaticPosition(
                potion.Transform,
                targetWorldPos,
                height: 2f,
                duration: moveDuration,
                onComplete: () => { outputStack.PushStack(potion); }
            );

            yield return moveWait; 
        }

        state = CauldronState.Idle;
    }

    public ItemStacker GetInputStacker() => inputStack;
    public ItemStacker GetOutputStacker() => outputStack;
}