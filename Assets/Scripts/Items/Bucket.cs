using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bucket : Item
{
    public override void Initialize(Vector2 startPosition, itemType type)
    {
        visualPrefab = Resources.Load("Items/bucket") as GameObject;

        base.Initialize(startPosition, type);

        //water or something idk
    }

    public override void Interact(InputAction.CallbackContext context)
    {
        base.Interact(context);

        //this is the current tile the player is standing on


        //if tile is flower: dig flower
        if (BlackBoard.currentTile == null || BlackBoard.currentTile.flower == null)
        {
            //do nothing, there is no flower
            return;
        }

        BlackBoard.currentTile.flower.ApplyWater();

        //dig or something idk
    }
}
