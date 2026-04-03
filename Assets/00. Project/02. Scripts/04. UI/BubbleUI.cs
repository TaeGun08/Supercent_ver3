using TMPro;
using UnityEngine;

public class BubbleUI : MonoBehaviour
{
    private Camera mainCam;
    
    [SerializeField] private TMP_Text bubbleText;
    [SerializeField] private Vector3 offset = new Vector3(0, 2.5f, 0);
    
    private Transform target;

    private void Awake()
    {
        mainCam = Camera.main;
    }
    
    public void Show(Transform newTarget, string text)
    {
        target = newTarget;
        bubbleText.text = text;
        gameObject.SetActive(true);
    }

    public void UpdateText(string text)
    {
        bubbleText.text = text;
    }

    public void Hide()
    {
        target = null;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (target == null) return;
        
        transform.position = target.position + offset; 
        
        transform.forward = mainCam.transform.forward; 
    }
}
