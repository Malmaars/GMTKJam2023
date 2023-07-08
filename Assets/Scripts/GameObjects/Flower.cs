using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Random = UnityEngine.Random;

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
    [Range(0f, 1f)] public float diseaseChance;
    public Color colorDiseased;
    public int recoveryRate = 10;
    public int crowHarmRate = 25;
    public int harvestPower; 
    
    [Header("Variables | Cycles")]
    public int cyclesPerGrowthStage;

    private SpriteRenderer sr;
    private FlowerState _state;
    private FlowerGrowState _growState;
    private bool _isDiseased;
    private bool _isBeingCrowed;
    private int _cyclesPassed;
    private int _health;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        _growState = FlowerGrowState.Seedling;
        _state = FlowerState.Normal;
        _isDiseased = false;
        _isBeingCrowed = false;
        _cyclesPassed = 0;
        _health = 100;

        UpdateSprite();
    }

    void Start()
    {

    }

    void Update()
    {
        if (_state == FlowerState.Normal && _health <= 0)
        {
            Die();
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
        else
            sr.color = Color.white;
    }

    public void NextCycle()
    {
        _cyclesPassed++;
        
        if (_state != FlowerState.Normal) 
            return;

        if (_cyclesPassed % cyclesPerGrowthStage == 0
            && _growState < FlowerGrowState.Grown)
        {
            _growState++;
        }

        if (_isBeingCrowed) {
            _health -= crowHarmRate;
            print("AUW! " + _health);
        }
        else
        {
            if (_health + recoveryRate <= 100)
                _health += recoveryRate;
            else
                _health = 100;
        }

        UpdateSprite();
    }

    public void Prune()
    {
        _isDiseased = false;
        UpdateSprite();
    }
    
    public void Harvest()
    {
        if (_state == FlowerState.Harvested)
            return;
            
        _state = FlowerState.Harvested;
        body.gravityScale = 3.2f;
        body.AddForce(Vector2.up * (harvestPower * 200));
        
        int dir = Random.Range(0, 2);
        if (dir == 0)
            body.AddForce(Vector2.right * (harvestPower * 80));
        if (dir == 1)
            body.AddForce(Vector2.left * (harvestPower * 80));
        
        body.AddTorque(harvestPower * 200);
        //ImpactController.instance.CreateImpact(1);
        StartCoroutine(CameraShaker.ShakeCamera(0.2f, 1));
        StartCoroutine(FlyAway());
    }

    private IEnumerator FlyAway()
    {
        for (float i = 0; i < 3; i += 1.0f / 60)
        {
            transform.localScale = new Vector3(1 + i * 2, 1 + i * 2, 1 + i * 2); 
            yield return new WaitForSeconds(1.0f / 60);
        }
        
        Destroy(gameObject);
    }

    public void Die()
    {
        _state = FlowerState.Dead;
        UpdateSprite();
    }

    public FlowerState GetFlowerState()
    {
        return _state;
    }
    
    public FlowerGrowState GetFlowerGrowState()
    {
        return _growState;
    }

    public void CrowEnter()
    {
        _isBeingCrowed = true;
    }
    
    public void CrowLeave()
    {
        _isBeingCrowed = false;
    }

    public bool IsBeingCrowed()
    {
        return _isBeingCrowed;
    }
}