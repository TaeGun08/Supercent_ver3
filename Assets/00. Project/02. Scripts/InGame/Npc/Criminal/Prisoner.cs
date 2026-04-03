using System.Collections;
using UnityEngine;

public enum PrisonerState { MovingToTableLine, WaitingInTableLine, TakingPotions, MovingToPrison, WaitingForPrisonSpace }

public class Prisoner : Npc
{
    [Header("Prisoner Settings")]
    [SerializeField] private ItemStacker myStacker;
    [SerializeField] private GameObject headObject;
    private int requiredPotionCount;
    
    private int currentPotionCount = 0;
    private PrisonerState state;
    private PotionTable targetTable;

    protected override void Awake()
    {
        base.Awake();
        
        if (headObject != null) headObject.SetActive(false);
    }

    private void Start()
    {
        targetTable = FindFirstObjectByType<PotionTable>();

        StartCoroutine(PrisonerRoutine());
    }

    private IEnumerator PrisonerRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        
        requiredPotionCount = Random.Range(1, 6);
        state = PrisonerState.MovingToTableLine;
        WaitingLineManager.Instance.JoinLine(this);
        state = PrisonerState.WaitingInTableLine;

        while (currentPotionCount < requiredPotionCount)
        {
            if (WaitingLineManager.Instance.GetFrontNpc() == this)
            {
                if (targetTable.CanDistribute)
                {
                    state = PrisonerState.TakingPotions;
                    yield return StartCoroutine(ExecuteTakingPotion(targetTable.GetStacker()));
                }
            }
            yield return wait;
        }

        if (headObject != null) headObject.SetActive(true);

        WaitingLineManager.Instance.OnFrontPersonLeft();
        
        state = PrisonerState.MovingToPrison;
        PrisonManager.Instance.TryEnterPrison(this);
    }

    private IEnumerator ExecuteTakingPotion(ItemStacker source)
    {
        IPickupAble potion = source.PopStack();
        if (potion == null) yield break;

        bool moveComplete = false;
        
        DOParabolicMove.MoveToDynamicTarget(
            potion.Transform,
            myStacker.transform,
            height: 1f,
            duration: 0.1f,
            yOffset: myStacker.CurrentCount * myStacker.ItemHeight,
            onComplete: () =>
            {
                myStacker.PushStack(potion);
                currentPotionCount++;
                moveComplete = true;
            }
        );

        yield return new WaitUntil(() => moveComplete);
        yield return new WaitForSeconds(0.2f);
    }
}