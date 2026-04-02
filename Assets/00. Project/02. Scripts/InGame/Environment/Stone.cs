using UnityEngine;

public class Stone : MonoBehaviour, IMiningTarget
{
    public Transform Transform => transform;
    
    [Header("Cobblestone")]
    [SerializeField] private Cobblestone cobblestone;
    
    public Cobblestone MineResource()
    {
        gameObject.SetActive(false);
        return Instantiate(cobblestone, transform.position, Quaternion.identity);
    }
}