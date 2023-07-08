using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CrowSpawner : MonoBehaviour
{
    [Header("References")] 
    public Crow crowPrefab;
    
    [Header("Variables")] 
    public int spawnInterval;

    private float _timeLastSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        _timeLastSpawn = Time.time;
    }

    void SpawnCrow()
    {
       Crow crow = Instantiate(crowPrefab, Vector2.up * 5, quaternion.identity);
       //crow.targetFlower = 
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _timeLastSpawn >= spawnInterval)
        {
            _timeLastSpawn = Time.time; 
            SpawnCrow();
        }
    }
}
