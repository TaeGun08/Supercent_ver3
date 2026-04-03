using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    [Header("Prisoner UI")]
    [SerializeField] private PrisonerUI prisonerUI;
    
    [Header("Gold UI")]
    [SerializeField] private GoldUI goldUI;

    [Header("Max UI")]
    [SerializeField] private MaxUI maxUI;

    public void ShowMaxUI(Transform target)
    {
        if (maxUI == null) return;
        maxUI.Play(target);
    }

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
        Debug.Log("Hide");
        prisonerUI.Hide();
    }

    public void UpdateGoldUI(int gold)
    {
        goldUI.UpdateText(gold);
    }
}
