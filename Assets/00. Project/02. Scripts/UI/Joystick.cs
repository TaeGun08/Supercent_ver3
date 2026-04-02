using UnityEngine;

public class Joystick : MonoBehaviour, IInputProvider
{
    [Header("JoyController")]
    [SerializeField] private RectTransform background;  
    [SerializeField] private RectTransform handle;      

    private Vector3 startPos;
    
    private float radius;
    private Vector3 dragDirection;
    
    public bool IsInputActive { get; private set; }
    public Vector3 MoveDirection => GetDragDirection();

    private void Start()
    {
        background.gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        radius = background.sizeDelta.x * 0.5f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartStick();

        if (Input.GetMouseButton(0))
            DragStick();

        if (Input.GetMouseButtonUp(0))
            EndStick();
    }
    
    /// <summary>
    /// 클릭을 시작했을 때
    /// </summary>
    private void StartStick()
    {
        IsInputActive = true;
        
        startPos = Input.mousePosition;
        background.gameObject.SetActive(true);

        background.position = startPos;
        handle.position = startPos;
    }
    
    /// <summary>
    /// 드래그 중일 때
    /// </summary>
    private void DragStick()
    {
        Vector3 direction = Input.mousePosition - startPos;

        if (direction.magnitude > radius)
            direction = direction.normalized * radius;

        handle.position = startPos + direction;

        dragDirection = handle.position - startPos;
    }
    
    /// <summary>
    /// 화면에 손을 땠을 때
    /// </summary>
    private void EndStick()
    {
        IsInputActive = false;
        
        background.gameObject.SetActive(false);
    }

    private Vector3 GetDragDirection()
    {
        Vector3 normalized = new Vector3(dragDirection.x, 0, dragDirection.y) / radius;
        return Vector3.ClampMagnitude(normalized, 1).normalized;
    }
}
