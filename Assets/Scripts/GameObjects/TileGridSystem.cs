using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGridSystem : MonoBehaviour
{
    public SoilTile[] soilTiles;

    private void Awake()
    {
        soilTiles = transform.GetComponentsInChildren<SoilTile>();
        BlackBoard.allTiles = soilTiles;
    }
}
