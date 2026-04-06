using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    private Vector3 punchScale= new Vector3(0.7f, 0.7f, 0.7f);
    private float duration = 0.4f;
    private int vibrato = 5;
    private float elasticity = 1f;

    private void Start()
    {
        transform.DOPunchScale(punchScale, duration, vibrato, elasticity);
    }
}
