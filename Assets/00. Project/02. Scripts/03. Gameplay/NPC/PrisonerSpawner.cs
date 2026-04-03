using System.Collections;
using UnityEngine;

public class PrisonerSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Prisoner prisonerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 5f;

    [Header("Dependencies")]
    [SerializeField] private PotionTable potionTable;

    private void Awake()
    {
        PoolManager.Instance.CreatePool(prisonerPrefab, initialCount: 5);
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnInterval);
        
        while (true)
        {
            if (PrisonManager.Instance.IsFull == false && WaitingLineManager.Instance.CanJoin)
            {
                SpawnPrisoner();
            }

            yield return wait;
        }
    }

    private void SpawnPrisoner()
    {
        Prisoner newPrisoner = PoolManager.Instance.Get<Prisoner>();
        if (newPrisoner == null) return;
        newPrisoner.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        newPrisoner.gameObject.SetActive(true);

        int needed = Random.Range(1, 4);
        newPrisoner.Initialize(potionTable, needed);
    }
}