using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    [Header("Player")]
    [SerializeField] private PlayerController player;
    public PlayerController Player => player;

    protected override void Awake()
    {
        base.Awake();

        if (player == null) player = FindFirstObjectByType<PlayerController>();
    }
}
