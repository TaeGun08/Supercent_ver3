using System.Collections.Generic;
using UnityEngine;

public class StoneGenerator : MonoBehaviour
{
    [Header("Generator Settings")] [SerializeField]
    private Stone stonePrefab;

    [SerializeField] private Transform fieldContainer;
    [SerializeField] private float respawnDelay = 3f;

    [Header("Grid Settings")] [SerializeField]
    private int rows = 7;

    [SerializeField] private int columns = 15;
    [SerializeField] private float spacingX = 1f;
    [SerializeField] private float spacingZ = 1f;

    private struct RespawnData
    {
        public Stone TargetStone;
        public float RespawnTime;
    }

    private Queue<RespawnData> respawnQueue = new Queue<RespawnData>();

    private void Start()
    {
        GenerateStones();
    }

    private void Update()
    {
        if (respawnQueue.Count == 0) return;

        if (Time.time < respawnQueue.Peek().RespawnTime) return;
        Stone stoneToRespawn = respawnQueue.Dequeue().TargetStone;

        if (stoneToRespawn == null) return;
        stoneToRespawn.gameObject.SetActive(true);
    }

    public void GenerateStones()
    {
        float startX = -(columns - 1) * spacingX / 2f;
        float startZ = -(rows - 1) * spacingZ / 2f;

        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 localSpawnPos = new Vector3(startX + (x * spacingX), 0, startZ + (z * spacingZ));
                Vector3 finalSpawnPos = transform.position + localSpawnPos;

                Stone stone = Instantiate(stonePrefab, finalSpawnPos, Quaternion.identity, fieldContainer);

                stone.Init(this);
            }
        }
    }

    /// <summary>
    /// 돌이 채굴되었을 때 호출되어 부활 대기열에 추가함
    /// </summary>
    public void RegisterRespawn(Stone stone)
    {
        respawnQueue.Enqueue(new RespawnData
        {
            TargetStone = stone,
            RespawnTime = Time.time + respawnDelay
        });
    }
}