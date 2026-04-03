using UnityEngine;

public enum MiningType { Burst, Continuous }

[CreateAssetMenu(fileName = "MiningData", menuName = "ScriptableObjects/MiningData", order = 2)]
public class MiningDataSO : ScriptableObject
{
    [Header("Mining Settings")]
    public MiningType Type = MiningType.Burst;
    public float MiningDelay = 1f;
    public float MiningRange = 2f;
    public int MiningPower = 1;

    [Header("Visual Settings")]
    public string AnimationName = "Mining";
    public GameObject EffectPrefab;
}