using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum itemType
{
    shovel,
    bucket,
    rake
}

public class Item
{
    public string animTriggerName;
    
    public virtual void Initialize(Vector2 startPosition, itemType type)
    {
        visual = Object.Instantiate(visualPrefab, startPosition, new Quaternion(0, 0, 0, 0));
        visual.GetComponent<itemRef>().thisItem = this;
        rb = visual.GetComponent<Rigidbody2D>();
    }

    public virtual void OnPickUp()
    {
        visual.GetComponent<SpriteRenderer>().enabled = false;
    }
    public virtual void OnThrow()
    {
        visual.GetComponent<SpriteRenderer>().enabled = true;
    }



    public virtual void LogicUpdate() { }

    public virtual void Interact(InputAction.CallbackContext context) { }
    public virtual void Release(InputAction.CallbackContext context) { }

    public GameObject visualPrefab;
    public GameObject visual;
    public Rigidbody2D rb;
}
