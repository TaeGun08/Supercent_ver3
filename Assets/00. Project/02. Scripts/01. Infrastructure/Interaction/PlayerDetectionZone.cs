using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetectionZone : MonoBehaviour
{
    public Action<bool> OnPlayerDetected { get; set; }
    private int playerLayer;
    private int detectedCollidersCount;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        OnPlayerDetected?.Invoke(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        OnPlayerDetected?.Invoke(false);
    }
}