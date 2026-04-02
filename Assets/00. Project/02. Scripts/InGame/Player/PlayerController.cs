using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerData
{
    public float MoveSpeed;
    public float Gravity;
}

public class PlayerController : MonoBehaviour
{
    private CharacterController charCtr;

    [Header("Joystick")]
    [SerializeField] private MonoBehaviour inputSource;

    public IInputProvider InputProvider { get; private set; }

    [Header("PlayerData")] 
    [SerializeField] private PlayerData playerData;

    private float verticalVelocity;
    
    private void Awake()
    {
        InputProvider = inputSource as IInputProvider;
        charCtr = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector3 velocity = Vector3.zero;

        if (InputProvider.IsInputActive)
        {
            Vector3 inputDir = InputProvider.MoveDirection;

            if (inputDir.sqrMagnitude > 0.01f)
            {
                Vector3 moveDir = inputDir.normalized;
                velocity += moveDir * playerData.MoveSpeed;

                transform.forward = moveDir;
            }
        }

        UpdateGravity();

        velocity.y = verticalVelocity;

        charCtr.Move(velocity * Time.deltaTime);
    }

    private void UpdateGravity()
    {
        if (charCtr.isGrounded) verticalVelocity = -1f;
        else verticalVelocity -= playerData.Gravity * Time.deltaTime;
    }
}