using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static readonly int IS_RUN = Animator.StringToHash("IsRun");
    public static readonly int PICKAX_MINING = Animator.StringToHash("Pickax_Mining");

    private IInputProvider inputProvider;

    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        inputProvider = GetComponent<PlayerController>().InputProvider;
    }

    private void Update()
    {
        PlayRunAnimation();
    }

    private void PlayRunAnimation()
    {
        animator.SetBool(IS_RUN, inputProvider.IsInputActive);
    }

    public void PlayMiningAnimation()
    {
        animator.SetTrigger(PICKAX_MINING);
    }
}
