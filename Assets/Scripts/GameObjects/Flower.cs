using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowerGrowState
{
   Seedling, Growing, Grown 
}

public enum FlowerState
{
   Normal, Harvested, Dead 
}

public class Flower : MonoBehaviour
{
    [Header("References")] 
    public Sprite spriteSeedling;
    public Sprite spriteGrowing;
    public Sprite spriteGrown;
    public Sprite spriteDead;
    public Rigidbody2D body;
    
    [Header("Variables")]
    public int hydrationDeclineRate;
    public int hydrationApplyRate;
    [Range(0f, 1f)] public float diseaseChance;
    public Color colorDiseased;
    public int harvestPower; //TODO: make relative to rage
    
    [Header("Variables | Cycles")]
    public float cycleTime;
    public int cyclesPerGrowthStage;

    private SpriteRenderer sr;
    private FlowerState _state;
    private FlowerGrowState _growState;
    private int _hydrationLevel; // Between 0 and 100
    private float _lastCycleTime;
    private bool _isDiseased;
    private int _cyclesPassed;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        _growState = FlowerGrowState.Seedling;
        _state = FlowerState.Normal;
        _hydrationLevel = 100;
        _isDiseased = false;
        _lastCycleTime = Time.time;
        _cyclesPassed = 0;
        
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
        if (_state == FlowerState.Dead)
        {
            sr.sprite = spriteDead;
            return;
        }
        
        if (_state == FlowerState.Harvested) 
        {
            sr.sprite = spriteDead; //TODO: harvested sprite with roots
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
        else
            sr.color = Color.white;

        float h, s, v;
        Color.RGBToHSV(sr.color, out h, out s, out v);
        v = (_hydrationLevel * 0.01f) + 0.5f;
        if (v > 1) v = 1;
        sr.color = Color.HSVToRGB(h, s, v);
    }

    void NextCycle()
    {
        _cyclesPassed++;
        
        if (_state == FlowerState.Harvested) 
            return;
        
        if (_isDiseased  || _hydrationLevel <= 0)
        {
            _state = FlowerState.Dead;
            return;
        }
    
        _hydrationLevel -= hydrationDeclineRate;

        if (_cyclesPassed % cyclesPerGrowthStage == 0
            && _growState < FlowerGrowState.Grown)
        {
            _growState++;
        }
    }

    public void ApplyWater()
    {
        _hydrationLevel += hydrationApplyRate;
        UpdateSprite();
    }
    
    public void Prune()
    {
        _isDiseased = false;
        UpdateSprite();
    }
    
    public void Harvest()
    {
        //TODO: change direction based on harvesting direction
        _state = FlowerState.Harvested;
        body.gravityScale = 3.2f;
        body.AddForce(Vector2.up * (harvestPower * 200));
        body.AddForce(Vector2.right * (harvestPower * 80));

        body.AddTorque(harvestPower * 200);
        ImpactController.instance.CreateImpact(1);

        StartCoroutine(FlyAway());

        //TODO: apply points
    }

    private IEnumerator FlyAway()
    {
        for (float i = 0; i < 3; i += 1.0f / 60)
        {
            transform.localScale = new Vector3(1 + i, 1 + i, 1 + i); 
            yield return new WaitForSeconds(1.0f / 60);
        }
        
        Destroy(gameObject);
    }
}
