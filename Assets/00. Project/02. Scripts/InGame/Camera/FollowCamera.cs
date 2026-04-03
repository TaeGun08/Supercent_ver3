using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform targetTrs;
    private Vector3 offsetPos;

    private void Start()
    {
        targetTrs = GameManager.Instance.Player.Transform;
        offsetPos = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = targetTrs.position + offsetPos;
    }
}
