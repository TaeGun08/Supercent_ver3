using UnityEngine;

public class Stone : MonoBehaviour, IMiningTarget
{
    public Transform Transform => transform;
    
    [Header("Cobblestone")]
    [SerializeField] private Cobblestone cobblestone;
    
    private StoneGenerator stoneGenerator;

    public void Init(StoneGenerator generator)
    {
        this.stoneGenerator = generator;
    }

    public Cobblestone MineResource(bool isFull)
    {
        stoneGenerator?.RegisterRespawn(this);

        gameObject.SetActive(false);
        
        return isFull ? null : Instantiate(cobblestone, transform.position, Quaternion.identity);
    }
}