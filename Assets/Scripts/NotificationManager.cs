using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum notificationType
{
    nothing,
    water,
    crow,
    dead
}

public static class NotificationManager
{
    public static void SpawnNotif(Flower askingFlower, notificationType notifType)
    {
        Sprite notifSprite = null;
        switch (notifType)
        {
            case notificationType.water:
                notifSprite = Resources.Load<Sprite>("Notifications/WaterNotif");
                break;
            case notificationType.crow:
                notifSprite = Resources.Load<Sprite>("Notifications/CrowNotif");
                break;
            case notificationType.dead:
                notifSprite = Resources.Load<Sprite>("Notifications/DeadNotif");
                break;
        }

        //fetch a prefab and edit the sprite with the correct one
        askingFlower.notif.notificationVisual.SetActive(true);
        Debug.Log(notifSprite);
        askingFlower.notif.notificationVisual.GetComponentInChildren<SpriteRenderer>().sprite = notifSprite;
        askingFlower.notif.notifType = notifType;
    }
}

public struct Notif
{
    public Notif(GameObject visual, notificationType type)
    {
        notificationVisual = visual;
        notifType = type;
    }

    public GameObject notificationVisual;
    public notificationType notifType;
}
