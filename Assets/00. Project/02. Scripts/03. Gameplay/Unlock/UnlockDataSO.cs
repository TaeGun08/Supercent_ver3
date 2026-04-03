using UnityEngine;

[CreateAssetMenu(fileName = "UnlockData", menuName = "ScriptableObjects/UnlockData")]
public class UnlockDataSO : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string unlockId;
    [SerializeField] private int requiredGold;
    
    public string UnlockId => unlockId;
    public int RequiredGold => requiredGold;
}
