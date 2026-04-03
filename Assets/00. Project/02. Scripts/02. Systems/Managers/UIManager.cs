using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    [Header("Prisoner UI")]
    [SerializeField] private PrisonerUI prisonerUI;
    
    [Header("Gold UI")]
    [SerializeField] private GoldUI goldUI;

    public void ShowPrisonerUI(Transform target, int current, int total)
    {
        prisonerUI.Initialize(target, total);
        prisonerUI.UpdateCount(current);
        prisonerUI.Show();
    }

    public void UpdatePrisonerUI(int current)
    {
        prisonerUI.UpdateCount(current);
    }

    public void HidePrisonerUI()
    {
        prisonerUI.Hide();
    }

    public void UpdateGoldUI(int gold)
    {
        goldUI.UpdateText(gold);
    }
}
