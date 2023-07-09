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
    [SerializeField] private ScoreMeterControl scoreMeterCtrl;
    
    private int scoreMtp = 1;
    private float lastScoreMtpTime;
    private float lastFullRageTime;
    private bool isFullyRaging;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            rageBarCtrl = FindObjectOfType<SliderControl>();
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        IncreaseRageMeter(0.75f);
    }

    private void Update()
    {
        if (isFullyRaging && Time.time - lastFullRageTime > 2.5f)
        {
            isFullyRaging = false;
        }
        else if (!isFullyRaging)
        {
            if (_rageMeter > 0) _rageMeter -= 0.05f * Time.deltaTime;
            if (_rageMeter < 0) _rageMeter = 0;
        }
        
        rageBarCtrl.SetSliderValue(_rageMeter);

        if (Time.time - lastScoreMtpTime > 2.5f && scoreMtp > 1)
        {
            scoreMtp = 1;
        }
    }

    public void IncreaseRageMeter(float amount = 0.1f)
    {
        _rageMeter += amount;
        if (_rageMeter >= 1.0f)
        {
            _rageMeter = 1.0f;
            lastFullRageTime = Time.time;
            isFullyRaging = true;
        }
        
        rageBarCtrl.SetSliderValue(_rageMeter);
    }

    public int IncreaseScore(int baseAmount)
    {
        int result = (int)(baseAmount * (1.0f + _rageMeter)) * scoreMtp;
        scoreMeterCtrl.SetScore(score);
        
        scoreMtp++;
        lastScoreMtpTime = Time.time;

        score += result;
        return result;
    }
}
