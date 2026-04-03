using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    [Header("Prisoner UI")]
    [SerializeField] private PrisonerUI prisonerUI;
    public PrisonerUI PrisonerUI => prisonerUI;
}
