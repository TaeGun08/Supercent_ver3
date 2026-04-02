using UnityEngine;

public class Stone : MonoBehaviour, IMiningTarget
{
    public Transform Transform => transform;
    
    [Header("Cobblestone")]
    [SerializeField] private Cobblestone cobblestone;
    
    public Cobblestone MineResource(bool isFull)
    {
        gameObject.SetActive(false);
        return isFull ? null : Instantiate(cobblestone, transform.position, Quaternion.identity);
    }
}