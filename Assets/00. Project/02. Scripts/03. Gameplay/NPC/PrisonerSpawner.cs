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
            // 실제 감옥이 가득 차기 전까지는 계속 스폰 허용 (대기열 공간도 고려)
            if (PrisonManager.Instance.IsPrisonFull == false && WaitingLineManager.Instance.CanJoin)
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

    PrisonManager.Instance.RegisterNewPrisoner();

    int needed = Random.Range(1, 4);
    newPrisoner.Initialize(potionTable, needed);
}
}