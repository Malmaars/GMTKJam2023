using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum notificationType
{
    water,
    crow,
    dead
}

public class NotificationManager : MonoBehaviour
{
    public Sprite waterSprite, crowSprite, deadSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNotif(Flower askingFlower, notificationType notifType)
    {

        Sprite notifSprite = null;
        switch (notifType)
        {
            case notificationType.water:
                notifSprite = waterSprite;
                break;
            case notificationType.crow:
                notifSprite = crowSprite;
                break;
            case notificationType.dead:
                notifSprite = deadSprite;
                break;

        }
    }
}
