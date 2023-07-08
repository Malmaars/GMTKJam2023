using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TMP_Text scoreLabel;
    public int score;
    
    private float _rageMeter;
    
    private SliderControl rageBarCtrl;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            rageBarCtrl = FindObjectOfType<SliderControl>();
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (_rageMeter > 0) _rageMeter -= 0.05f * Time.deltaTime;
        if (_rageMeter < 0) _rageMeter = 0;
        
        rageBarCtrl.SetSliderValue(_rageMeter);
    }

    public void IncreaseRageMeter(float amount = 0.1f)
    {
        _rageMeter += amount;
        if (_rageMeter > 1.0f) _rageMeter = 1.0f;
        
        rageBarCtrl.SetSliderValue(_rageMeter);
    }

    public void IncreaseScore(int baseAmount)
    {
        score += (int)(baseAmount * (1.0f + _rageMeter));
    }
}
