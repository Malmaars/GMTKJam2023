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
    public virtual void Initialize(Vector2 startPosition, itemType type)
    {
        visual = Object.Instantiate(visualPrefab, startPosition, new Quaternion(0, 0, 0, 0));
        rb = visual.GetComponent<Rigidbody2D>();
    }

    public virtual void Interact(InputAction.CallbackContext context) { }

    public GameObject visualPrefab;
    public GameObject visual;
    public Rigidbody2D rb;
}
