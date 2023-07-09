using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGridSystem : MonoBehaviour
{
    private SoilTile currentTile;
    public SoilTile[] soilTiles;

    private void Awake()
    {
        soilTiles = transform.GetComponentsInChildren<SoilTile>();
        BlackBoard.allTiles = soilTiles;
    }

    private void Update()
    {
        if(currentTile != BlackBoard.currentTile)
        {
            if (currentTile != null)
            {
                //hightlight the new tile, and unhiglight the previous tile
            }

            if (BlackBoard.currentTile != null)
            {
                
            }
            currentTile = BlackBoard.currentTile;
        }
    }
}
