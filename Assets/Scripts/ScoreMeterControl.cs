using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreMeterControl : MonoBehaviour
{
    private TMP_Text textObject;
    public float wiggleSpeed;
    public float amplification;
    public Animator anim;
    
    private void Awake()
    {
        textObject = GetComponentInChildren<TMP_Text>();
    }
    
    void Start()
    {
        SetScore(0);
        wiggleSpeed = 40f;
        amplification = 3f;
    }
    
    public void SetScore(int score)
    {
        anim.Play("ScoreSize");;
        textObject.text = score.ToString();
    }
    
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0,(Mathf.Sin(Time.time*wiggleSpeed)*amplification));
    }
}
