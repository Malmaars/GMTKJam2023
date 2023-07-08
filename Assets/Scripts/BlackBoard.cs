using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//blackboard exists for info that is public property
public static class BlackBoard
{
    public static List<Item> allItems;
    public static SoilTile[] allTiles;

    public static SoilTile currentTile;

    public static void OnAwake() {
        allItems = new List<Item>();
    }

    public static void SpawnItem(Vector2 spawnPosition, itemType type)
    {
        Item newItem = null;
        switch (type)
        {
            case itemType.shovel:
                newItem = new Shovel();
                break;
            case itemType.bucket:
                newItem = new Bucket();
                break;
            case itemType.rake:
                newItem = new Rake();
                break;
        }

        newItem.Initialize(spawnPosition, type);
        allItems.Add(newItem);
    }
}
