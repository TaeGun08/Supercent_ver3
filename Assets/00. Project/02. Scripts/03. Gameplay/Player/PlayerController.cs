using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayer
{
    private CharacterController controller;
    private IInputProvider inputProvider;

    public Transform Transform => transform;

    [Header("Dependencies")]
    [SerializeField] private PlayerDataSO playerData;

    private float verticalVelocity;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputProvider = FindFirstObjectByType<Joystick>();
    }

    public IInputProvider InputProvider => inputProvider;

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector3 velocity = Vector3.zero;

        if (inputProvider.IsInputActive)
        {
            Vector3 inputDir = inputProvider.MoveDirection;

            if (inputDir.sqrMagnitude > 0.01f)
            {
                Vector3 moveDir = inputDir.normalized;
                velocity += moveDir * playerData.MoveSpeed;

                transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * playerData.RotationSpeed);
            }
        }

        UpdateGravity();

        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    private void UpdateGravity()
    {
        if (controller.isGrounded) verticalVelocity = -1f;
        else verticalVelocity -= playerData.Gravity * Time.deltaTime;
    }
}