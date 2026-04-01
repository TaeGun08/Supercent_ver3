using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public float MoveSpeed;
    public float Gravity;
    public float PickupSpeed;
    public float PutDownSpeed;
}

public class PlayerController : MonoBehaviour
{
    private static readonly int IS_RUN = Animator.StringToHash("IsRun");
    
    private GameManager gameManager;
    private UIManager uIManager;

    private Joystick joystick;

    private CharacterController charCtr;
    private Animator animator;

    [Header("Player Settings")] [SerializeField]
    private PlayerStat playerStat;

    private float verticalVelocity;
    
    private void Awake()
    {
        gameManager = GameManager.Instance;
        uIManager = UIManager.Instance;

        joystick = uIManager.Joystick;

        charCtr = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector3 velocity = Vector3.zero;
        
        if (joystick.IsStickPressed)
        {
            Vector3 inputDir = joystick.GetDragDirection();

            if (inputDir.sqrMagnitude > 0.01f)
            {
                Vector3 moveDir = inputDir.normalized;
                velocity += moveDir * playerStat.MoveSpeed;

                transform.forward = moveDir;
            }
        }

        UpdateGravity();
        
        velocity.y = verticalVelocity;
        
        charCtr.Move(velocity * Time.deltaTime);
        
        animator.SetBool(IS_RUN, joystick.IsStickPressed);
    }

    private void UpdateGravity()
    {
        if (charCtr.isGrounded) verticalVelocity = -1f; 
        else verticalVelocity -= playerStat.Gravity * Time.deltaTime;
    }
}