using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static readonly int IS_RUN = Animator.StringToHash("IsRun");
    public static readonly int PICKAX_MINING = Animator.StringToHash("Pickax_Mining");

    private PlayerController player;

    private Animator animator;

    private bool isRun;
    
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();

        isRun = player.InputProvider.IsInputActive;
    }

    private void Update()
    {
        PlayRunAnimation();
    }

    private void PlayRunAnimation()
    {
        animator.SetBool(IS_RUN, isRun);
    }

    public void PlayMiningAnimation()
    {
        animator.SetTrigger(PICKAX_MINING);
    }
}
