using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InQuarry : MonoBehaviour
{
    private int playerLayer;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        if (other.TryGetComponent(out MiningController controller))
        {
            controller.SetMiningZoneActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;

        if (other.TryGetComponent(out MiningController controller))
        {
            controller.SetMiningZoneActive(false);
        }
    }
}
