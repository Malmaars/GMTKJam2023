using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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
       int randomFlowerIndex = Random.Range(0, BlackBoard.allTiles.Length - 1);
       Flower targetFlower = BlackBoard.allTiles[randomFlowerIndex].flower;

       if (!targetFlower)
           return;
           
       if (targetFlower.GetFlowerState() == FlowerState.Normal
           && targetFlower.GetFlowerGrowState() != FlowerGrowState.Seedling
           && !targetFlower.IsBeingCrowed())
       {
           Crow crow = Instantiate(crowPrefab, GetRandomSpawnPoint(), quaternion.identity);
           crow.targetFlower = targetFlower;
       }
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

    private Vector2 GetRandomSpawnPoint()
    {
        int x = 0, y = 0, side;
        side = Random.Range(0, 4);
        
        if (side == 0) {
            x = -26;
            y = Random.Range(-16, 16);
        } else if (side == 1) {
            x = 26;
            y = Random.Range(-16, 16);
        } else if (side == 2) {
            x = Random.Range(-26, 26);
            y = -16;
        } else if (side == 3) {
            x = Random.Range(-26, 26);
            y = 16;
        }

        return new Vector2(x, y);
    }
}
