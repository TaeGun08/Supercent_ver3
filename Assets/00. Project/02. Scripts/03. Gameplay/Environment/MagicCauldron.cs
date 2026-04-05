using System.Collections;
using UnityEngine;

public enum CauldronState { Idle, Crafting }

public class MagicCauldron : MonoBehaviour
{
    private static readonly int CRAFT = Animator.StringToHash("Craft");
    
    [Header("Dependencies")] 
    [SerializeField] private ItemStacker inputStack;
    [SerializeField] private ItemStacker outputStack;
    [SerializeField] private Potion potionPrefab;

    [Header("Settings")]
    [SerializeField] private float craftDuration = 3f;
    private float moveDuration = 0.3f;

    private CauldronState state = CauldronState.Idle;
    private Animator animator;
    private WaitForSeconds craftWait;
    private WaitForSeconds moveWait;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        PoolManager.Instance.CreatePool(potionPrefab, initialCount: 5);

        craftWait = new WaitForSeconds(craftDuration);
        moveWait = new WaitForSeconds(moveDuration);
    }

    private void OnEnable()
    {
        // [Event-Driven]: 스택 변화를 감시하여 능동적으로 제작 시작
        if (inputStack != null) inputStack.OnStackChanged += CheckAndStartCrafting;
        if (outputStack != null) outputStack.OnStackChanged += CheckAndStartCrafting;
    }

    private void OnDisable()
    {
        if (inputStack != null) inputStack.OnStackChanged -= CheckAndStartCrafting;
        if (outputStack != null) outputStack.OnStackChanged -= CheckAndStartCrafting;
    }

    private void CheckAndStartCrafting()
    {
        // 제작 조건: Idle 상태이고, 재료가 있으며, 결과물 보관소에 자리가 있을 때
        if (state == CauldronState.Idle && inputStack.HasItem && !outputStack.IsFull)
        {
            // [Tutorial Hook]: 돌 투입 감지
            TutorialManager.Instance.OnActionPerform(TutorialCondition.InputStone);
            StartCoroutine(CraftingRoutine());
        }
    }

    private IEnumerator CraftingRoutine()
    {
        state = CauldronState.Crafting;

        while (inputStack.HasItem && !outputStack.IsFull)
        {
            IPickupAble stone = inputStack.PopStack();
            if (stone != null)
            {
                DOParabolicMove.MoveToStaticPosition(
                    stone.Transform,
                    transform.position,
                    height: 3f,
                    duration: moveDuration,
                    onComplete: () => stone.Release()
                );
                yield return moveWait; 
            }

            animator.SetTrigger(CRAFT);
            yield return craftWait;

            Potion potion = PoolManager.Instance.Get<Potion>();
            if (potion != null)
            {
                potion.Transform.position = transform.position;
                Vector3 targetWorldPos = outputStack.transform.position + outputStack.GetNextLocalPosition();

                DOParabolicMove.MoveToStaticPosition(
                    potion.Transform,
                    targetWorldPos,
                    height: 2f,
                    duration: moveDuration,
                    onComplete: () => {
                        outputStack.PushStack(potion);
                        AudioManager.Instance.Play(SoundType.PotionProduced);
                    }
                );
                yield return moveWait; 
            }
        }

        state = CauldronState.Idle;
        // 루프 종료 후 남은 재료가 있는지 마지막으로 한 번 더 확인 (상태 불일치 방지)
        CheckAndStartCrafting();
    }

    public ItemStacker GetInputStacker() => inputStack;
    public ItemStacker GetOutputStacker() => outputStack;
}
