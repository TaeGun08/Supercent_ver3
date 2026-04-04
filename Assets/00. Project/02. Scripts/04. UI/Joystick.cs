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
    /// Ŭ���� �������� ��
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
    /// �巡�� ���� ��
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
    /// 화면에서 손을 뗐을 때 (외부 강제 중단 시에도 사용)
    /// </summary>
    public void EndStick()
    {
        IsInputActive = false;
        dragDirection = Vector3.zero;
        background.gameObject.SetActive(false);
    }

    private Vector3 GetDragDirection()
    {
        Vector3 normalized = new Vector3(dragDirection.x, 0, dragDirection.y) / radius;
        return Vector3.ClampMagnitude(normalized, 1).normalized;
    }
}
