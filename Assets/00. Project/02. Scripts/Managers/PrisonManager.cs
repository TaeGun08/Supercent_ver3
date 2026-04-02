using UnityEngine;

public class PrisonManager : SingletonBase<PrisonManager>
{
    [Header("Prison Settings")]
    [SerializeField] private int maxCapacity = 20;
    [SerializeField] private GameObject openContent;

    private int currentPrisonerCount;

    public bool IsFull => currentPrisonerCount >= maxCapacity;

    public void OpenContent()
    {
        openContent.SetActive(true);
        maxCapacity *= 2;
    }
    
    public void AddPrisoner()
    {
        currentPrisonerCount++;
    }
}