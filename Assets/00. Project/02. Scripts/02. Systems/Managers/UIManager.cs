using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    [Header("Prisoner UI")]
    [SerializeField] private PrisonerUI prisonerUI;

    public void ShowPrisonerUI(Transform target, int current, int total)
    {
        if (prisonerUI == null) return;
        prisonerUI.Initialize(target, total);
        prisonerUI.UpdateCount(current);
        prisonerUI.Show();
    }

    public void UpdatePrisonerUI(int current)
    {
        if (prisonerUI == null) return;
        prisonerUI.UpdateCount(current);
    }

    public void HidePrisonerUI()
    {
        if (prisonerUI == null) return;
        prisonerUI.Hide();
    }
}
