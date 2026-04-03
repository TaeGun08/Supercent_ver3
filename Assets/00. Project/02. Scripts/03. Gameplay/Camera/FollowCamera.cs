using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Vector3 offset;
    private Transform target;

    private void Start()
    {
        offset = transform.position;
        target = GameManager.Instance.Player.Transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + offset;
    }
}