using UnityEngine;

public class Stone : MonoBehaviour, IMiningTarget
{
    public Transform Transform => transform;
    
    private StoneGenerator stoneGenerator;

    public void Init(StoneGenerator generator)
    {
        stoneGenerator = generator;
    }

    public IPickupAble MineResource(bool isFull)
    {
        stoneGenerator?.RegisterRespawn(this);

        gameObject.SetActive(false);
        
        if (isFull) return null;

        Cobblestone newCobble = PoolManager.Instance.Get<Cobblestone>();

        newCobble.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        newCobble.gameObject.SetActive(true);

        return newCobble;
    }
}
