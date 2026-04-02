using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Npc : MonoBehaviour
{
    [Header("Npc Settings")] 
    [SerializeField] protected float moveSpeed = 2f;
    
    protected virtual void OnEnable()
    {
        StartCoroutine(NpcBehaviorCoroutine());
    }
    
    protected abstract IEnumerator NpcBehaviorCoroutine();
}
