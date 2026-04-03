using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayer
{
    private CharacterController charCtr;
    private IInputProvider inputProvider;

    public Transform Transform => transform;

    [Header("Dependencies")]
    [SerializeField] private MonoBehaviour inputSource;
    [SerializeField] private PlayerDataSO playerData;

    private float verticalVelocity;
    
    private void Awake()
    {
        // [Fail-Fast]: 필수 컴포넌트 및 데이터 주입 검증
        charCtr = GetComponent<CharacterController>();
        Debug.Assert(charCtr != null, "[PlayerController] CharacterController가 누락되었습니다.");
        
        inputProvider = inputSource as IInputProvider;
        Debug.Assert(inputProvider != null, "[PlayerController] IInputProvider가 inputSource에서 발견되지 않았습니다.");
        
        Debug.Assert(playerData != null, "[PlayerController] PlayerDataSO가 할당되지 않았습니다.");
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

                // 부드러운 회전 적용 (기존 직접 transform.forward 대신 Lerp 권장)
                transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * playerData.RotationSpeed);
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