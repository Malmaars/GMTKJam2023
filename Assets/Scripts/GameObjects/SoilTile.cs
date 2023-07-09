using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SoilTile : Interactable
{
    public GameObject Hightlight;
    public bool dug;
    public GameObject grassTile;

    public Flower flowerPrefab;
    
    public Flower flower;
    [Range(0, 3)] public int growthStage;
    public float cycleDuration = 3;
    public int hydrationDeclineRate = 5;
    public int hydrationApplyRate = 10;

    private Sprite[] sprites;
    public SpriteRenderer sr;
    
    private float _lastCycleTime;
    private int _hydrationLevel;

    public Transform thisTransform;
    
    [Header("Hydration sprites")]
    public Sprite spriteDehydrated;
    public Sprite spriteNormal;
    public Sprite spriteSlightlyWet;
    public Sprite spriteVeryWet;
    public Sprite spriteOverhydrated;
    
    [SerializeField] private TMP_Text pointsLabel;
    [SerializeField] private Animator pointsAnimator;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        thisTransform = GetComponent<Transform>();
        
        flower = GetComponentInChildren<Flower>();
    }

    public void Start()
    {
        _lastCycleTime = Time.time;
        _hydrationLevel = 45;
    }

    public void Update()
    {
        if (Time.time - _lastCycleTime >= cycleDuration)
        {
            _lastCycleTime = Time.time;
            
            if (flower && _hydrationLevel <= 0)
            {
                flower.Die();
                //flower = null;
                return;
            }

            if (flower != null)
                _hydrationLevel -= hydrationDeclineRate;

            if (flower) flower.NextCycle();
            UpdateSprite();
        }

        if (BlackBoard.currentTile == this)
        {
            Hightlight.SetActive(true);
        }
        else
        {
            Hightlight.SetActive(false);
        }
    }

    private void UpdateSprite()
    {
        switch (_hydrationLevel)
        {
            case >= 100:
                if (flower)
                {
                    flower.Die();
                    flower.transform.SetParent(null);
                    //flower = null;
                }
                sr.sprite = spriteOverhydrated;
                break;
            case >= 75:
                sr.sprite = spriteVeryWet;
                break;
            case >= 50:
                sr.sprite = spriteSlightlyWet;
                break;
            case >= 25:
                sr.sprite = spriteNormal;
                break;
            default:
                sr.sprite = spriteDehydrated;
                if (flower != null && !flower.dead)
                {
                    Debug.Log("Send water help");
                    NotificationManager.SpawnNotif(flower, notificationType.water);
                }
                break;
        }

        if (flower != null)
        {
            if (_hydrationLevel > 50)
            {
                flower.healthy = true;
            }

            else
            {
                flower.healthy = false;
            }
        }

        //float h, s, v;
        //Color.RGBToHSV(sr.color, out h, out s, out v);
        //v = (_hydrationLevel * 0.01f) + 0.5f;
        //if (v > 1) v = 1;
        //sr.color = Color.HSVToRGB(h, s, v);
    }
    
    public void ApplyWater()
    {
        _hydrationLevel += hydrationApplyRate;
        if (_hydrationLevel < 100)
        {
            IncreaseScore(hydrationApplyRate);
        }
        UpdateSprite();
    }

    public void IncreaseScore(int amount)
    {
        int points = ScoreManager.instance.IncreaseScore(amount);
        pointsLabel.text = "+" + points;
        pointsAnimator.SetTrigger("popup");
    }

    public void Harvest()
    {
        if (flower) flower.Harvest();

        _lastCycleTime = Time.time;
        _hydrationLevel = 20;

        if (flower.GetFlowerGrowState() == FlowerGrowState.Grown)
        {
            IncreaseScore(500);
            ScoreManager.instance.IncreaseRageMeter(0.25f);
        }
    
        else if (flower.GetFlowerState() == FlowerState.Dead)
        {
            IncreaseScore(100);
            ScoreManager.instance.IncreaseRageMeter(0.1f);
        }

        StartCoroutine(_PlantSeed());
    }

    private IEnumerator _PlantSeed()
    {
        yield return new WaitForSeconds(5);
        PlantSeed();
    }

    public void PlantSeed()
    {
        if (flower) return;
        
        flower = Instantiate(flowerPrefab, transform.position, Quaternion.identity);
        flower.transform.SetParent(transform);
        flower.OnPlanted();
        UpdateSprite();
        _hydrationLevel = 40;
    }

    public void DigTile()
    {
        dug = true;
        grassTile.SetActive(false);
        
        IncreaseScore(100);
    }
}