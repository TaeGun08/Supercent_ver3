using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Npc : MonoBehaviour
{
    [Header("Npc")] 
    [SerializeField] protected float moveSpeed = 2f;

    protected void OnEnable()
    {
        StartCoroutine(NpcBehaviorCoroutine());
    }
    
    protected abstract IEnumerator NpcBehaviorCoroutine();
}
