using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public LeaderBoardController leaderController;
    public GameObject leaderboardCanvas;
    public TextMeshProUGUI yourScore;

    public static ScoreManager instance;

    public TMP_Text scoreLabel;
    public int score;
    
    private float _rageMeter;
    
    private SliderControl rageBarCtrl;
    [SerializeField] private ScoreMeterControl scoreMeterCtrl;
    
    public int scoreMtp = 1;
    private float lastScoreMtpTime;
    private float lastFullRageTime;
    private bool isFullyRaging;
    private float lastRageIncreaseTime;
    private float rageDecrement = 0.05f;

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
        lastRageIncreaseTime = Time.time;
    }

    private void Update()
    {
        if (isFullyRaging && Time.time - lastFullRageTime > 1.5f)
        {
            isFullyRaging = false;
        }
        else if (!isFullyRaging)
        {
            if (_rageMeter > 0) _rageMeter -= rageDecrement * Time.deltaTime;
            if (_rageMeter < 0) _rageMeter = 0;
        }
        
        rageBarCtrl.SetSliderValue(_rageMeter);
        MusicPlayer.instance.OnRageMeterChange(_rageMeter);

        if (Time.time - lastScoreMtpTime > 3 && scoreMtp > 1)
        {
            scoreMtp = 1;
        }


        if (Time.time - lastRageIncreaseTime > 10)
        {
            rageDecrement += 0.01f;
            lastRageIncreaseTime = Time.time;
        }
        if(_rageMeter <= 0)
        {
            ActivateLeaderBoard();

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
        MusicPlayer.instance.OnRageMeterChange(_rageMeter);
    }

    public int IncreaseScore(int baseAmount)
    {
        int result = (int)(baseAmount * (1.0f + _rageMeter)) * scoreMtp;
        
        scoreMtp++;
        lastScoreMtpTime = Time.time;

        score += result;
        scoreMeterCtrl.SetScore(score);

        return result;
    }

    public void ActivateLeaderBoard()
    {
        yourScore.text = score.ToString();
        leaderboardCanvas.SetActive(true);
        leaderController.SetScore(score);
        FindObjectOfType<Player>().gameObject.SetActive(false);
    }
}
