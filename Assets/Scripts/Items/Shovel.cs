using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shovel : Item
{
    public override void Initialize(Vector2 startPosition, itemType type)
    {
        visualPrefab = Resources.Load("Items/shovel") as GameObject;
        animTriggerName = "pick-up-shovel";

        base.Initialize(startPosition, type);
    }
    public override void Interact(InputAction.CallbackContext context)
    {
        base.Interact(context);

        //this is the current tile the player is standing on

        //if tile is flower: dig flower
        if(BlackBoard.currentTile == null || BlackBoard.currentTile.flower == null)
        {
            //do nothing, there is no flower
            return;
        }

        BlackBoard.currentTile.Harvest();
        ScoreManager.instance.IncreaseRageMeter(0.2f);

        //dig or something idk
    }
}
