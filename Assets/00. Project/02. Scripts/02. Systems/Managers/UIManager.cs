using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    [Header("Target Canvas")]
    [SerializeField] private Canvas canvas;
    public Canvas Canvas => canvas;
}
