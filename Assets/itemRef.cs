using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemRef : MonoBehaviour
{
    public Item thisItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fence"))
        {
            thisItem.rb.velocity *= -1;
        }
    }
}
