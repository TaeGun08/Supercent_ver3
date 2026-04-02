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
        other.GetComponent<MiningController>().SetActiveEquipment(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer) return;
        other.GetComponent<MiningController>().SetActiveEquipment(false);
    }
}
