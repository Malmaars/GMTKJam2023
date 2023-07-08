using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowerGrowState
{
   Seedling, Growing, Grown 
}

public class Flower : MonoBehaviour
{
    [Header("References")] 
    public Sprite spriteSeedling;
    public Sprite spriteGrowing;
    public Sprite spriteGrown;
    public Sprite spriteDead;
    
    [Header("Variables")]
    public int hydrationDeclineRate;
    public int hydrationApplyRate;
    [Range(0f, 1f)] public float diseaseChance;
    public Color colorDiseased;
    
    [Header("Variables | Cycles")]
    public float cycleTime;
    public int cyclesPerGrowthStage;

    private SpriteRenderer sr;
    private FlowerGrowState _growState;
    private int _hydrationLevel; // Between 0 and 100
    private float _lastCycleTime;
    private bool _isDiseased;
    private bool _isAlive;

    private int cyclesPassed;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        _growState = FlowerGrowState.Seedling;
        _hydrationLevel = 100;
        _isDiseased = false;
        _isAlive = true;
        _lastCycleTime = Time.time;
        
        UpdateSprite();
    }

    void Update()
    {
        if (Time.time - _lastCycleTime >= cycleTime)
        {
            _lastCycleTime = Time.time;
            NextCycle();
            UpdateSprite();
        }
        
    }

    void UpdateSprite()
    {
        if (!_isAlive)
        {
            sr.sprite = spriteDead;
            return;
        }

        sr.sprite = _growState switch
        {
            FlowerGrowState.Seedling => spriteSeedling,
            FlowerGrowState.Growing => spriteGrowing,
            FlowerGrowState.Grown => spriteGrown,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (_isDiseased)
            sr.color = colorDiseased;

        float h, s, v;
        Color.RGBToHSV(sr.color, out h, out s, out v);
        s = _hydrationLevel * 0.01f;
        sr.color = Color.HSVToRGB(h, s, v);
    }

    void NextCycle()
    {
        cyclesPassed++;
        
        if (_isDiseased  || _hydrationLevel <= 0)
        {
            _isAlive = false;
            return;
        }
    
        _hydrationLevel -= hydrationDeclineRate;
        
        if (cyclesPassed % cyclesPerGrowthStage == 0
            && _growState >= FlowerGrowState.Grown) return;
        
        _growState++;
    }

    void ApplyWater()
    {
        _hydrationLevel += hydrationApplyRate;
        UpdateSprite();
    }
    
    void Prune()
    {
        _isDiseased = false;
        UpdateSprite();
    }

}
