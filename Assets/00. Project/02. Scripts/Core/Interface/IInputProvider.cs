using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputProvider
{
    public bool IsInputActive { get; }
    public Vector3 MoveDirection { get; }
}
