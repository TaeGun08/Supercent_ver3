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
    [SerializeField] private GameObject equipment;
    public GameObject Equipment => equipment;
    [SerializeField] protected float miningDelay;
    [SerializeField] protected MiningSensor miningSensor;
    protected float timer = float.MaxValue;
    
    public virtual void Init(MiningContext miningContext)
    {
        playerController = miningContext.PlayerController;
        playerAnimation = miningContext.PlayerAnimation;
        itemStacker = miningContext.ItemStacker;
    }
    
    public abstract void Mining();
}
