using UnityEngine;

[CreateAssetMenu(fileName = "MiningData", menuName = "ScriptableObjects/MiningData", order = 2)]
public class MiningDataSO : ScriptableObject
{
    [Header("Mining Settings")]
    public float MiningDelay = 1f;
    public float MiningRange = 2f;
    public int MiningPower = 1;
    
    [Header("Visual Settings")]
    public string AnimationTriggerName = "Mining";
    public GameObject EffectPrefab;
}