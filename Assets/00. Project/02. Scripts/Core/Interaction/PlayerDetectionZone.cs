using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetectionZone : MonoBehaviour
{
    [SerializeField] private UnityEvent<bool> onPlayerDetected;
    private int playerLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        Debug.Assert(playerLayer != -1, "[PlayerDetectionZone] 'Player' 레이어가 존재하지 않습니다.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            onPlayerDetected?.Invoke(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            onPlayerDetected?.Invoke(false);
        }
    }
}