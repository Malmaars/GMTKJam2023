using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SoilTile : Interactable
{
    public Flower flower;
    [Range(0, 3)] public int growthStage;
    public float cycleTime = 3;
    public int hydrationDeclineRate = 5;
    public int hydrationApplyRate = 10;

    private Sprite[] sprites;
    private SpriteRenderer sr;
    
    private float _lastCycleTime;
    private int _hydrationLevel;

    public Transform thisTransform;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        thisTransform = GetComponent<Transform>();
        
        flower = GetComponentInChildren<Flower>();
    }

    public void Start()
    {
        _lastCycleTime = Time.time;
        _hydrationLevel = 100;
    }

    public void Update()
    {
        if (Time.time - _lastCycleTime >= cycleTime)
        {
            _lastCycleTime = Time.time;
            
            if (_hydrationLevel <= 0)
            {
                flower.Die();
                return;
            }
    
            _hydrationLevel -= hydrationDeclineRate;
            
            flower.NextCycle();
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        //float h, s, v;
        //Color.RGBToHSV(sr.color, out h, out s, out v);
        //v = (_hydrationLevel * 0.01f) + 0.5f;
        //if (v > 1) v = 1;
        //sr.color = Color.HSVToRGB(h, s, v);
    }
    
    public void ApplyWater()
    {
        _hydrationLevel += hydrationApplyRate;
        UpdateSprite();
    }

    public void Harvest()
    {
        flower.Harvest();
    }
}