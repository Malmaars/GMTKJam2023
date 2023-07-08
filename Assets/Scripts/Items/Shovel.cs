using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shovel : Item
{
    public override void Initialize(Vector2 startPosition, itemType type)
    {
        visualPrefab = Resources.Load("Items/shovel") as GameObject;

        base.Initialize(startPosition, type);
    }
    public override void Interact(InputAction.CallbackContext context)
    {
        base.Interact(context);

        //dig or something idk
    }
}
