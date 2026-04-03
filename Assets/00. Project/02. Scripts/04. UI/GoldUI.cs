using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    public void UpdateText(int gold)
    {
        goldText.text = gold.ToString();
    }
}
