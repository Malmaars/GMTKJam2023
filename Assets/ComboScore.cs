using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboScore : MonoBehaviour
{
    public ScoreManager scoreManager;

    public TextMeshProUGUI comboText;

    private void Update()
    {
        UpdateCombo();
    }
    public void UpdateCombo()
    {
        comboText.text = "x" + scoreManager.scoreMtp.ToString();
    }
}
