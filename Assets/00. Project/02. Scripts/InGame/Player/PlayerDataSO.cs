using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class PlayerDataSO : ScriptableObject
{
    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    public float Gravity = 9.81f;
    public float RotationSpeed = 10f;
}