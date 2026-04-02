using UnityEngine;

public class MiningContext
{
    public PlayerController PlayerController;
    public PlayerAnimation PlayerAnimation;
    public ItemStacker ItemStacker;
}

public abstract class MiningEquipment : MonoBehaviour
{
    protected PlayerController playerController;
    protected PlayerAnimation playerAnimation;
    protected ItemStacker itemStacker;
    
    [Header("Equipment")]
    [SerializeField] protected float miningDelay;
    [SerializeField] protected MiningSensor miningSensor;
    protected float timer;
    
    public virtual void Init(MiningContext miningContext)
    {
        playerController = miningContext.PlayerController;
        playerAnimation = miningContext.PlayerAnimation;
        itemStacker = miningContext.ItemStacker;
    }
    
    public abstract void Mining();
}
