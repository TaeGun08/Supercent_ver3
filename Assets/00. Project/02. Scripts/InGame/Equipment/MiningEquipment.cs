using UnityEngine;

public class MiningEquipment : MonoBehaviour
{
    [Header("Equipment Data")]
    [SerializeField] private MiningDataSO miningData;
    public MiningDataSO MiningData => miningData;

    [SerializeField] private GameObject visualObject;
    public GameObject VisualObject => visualObject;

    [SerializeField] protected MiningSensor miningSensor;
    public MiningSensor MiningSensor => miningSensor;
}
