using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rake : Item
{
    public override void Interact(InputAction.CallbackContext context)
    {
        visualPrefab = Resources.Load("Items/rake") as GameObject;

        base.Interact(context);

        //do something idk
    }
}
