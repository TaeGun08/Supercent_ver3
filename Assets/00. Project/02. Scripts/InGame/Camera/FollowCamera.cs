using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameManager gameManager;

    private Transform targetTrs;
    private Vector3 offsetPos;

    private void Start()
    {
        gameManager = GameManager.Instance;
        
        offsetPos = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = targetTrs.position + offsetPos;
    }
}
