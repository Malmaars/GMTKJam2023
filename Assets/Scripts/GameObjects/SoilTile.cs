using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SoilTile : Interactable
{
    public Flower flower;
    [Range(0, 3)] public int growthStage;

    private Sprite[] sprites;
    private SpriteRenderer sr;

    public Transform thisTransform;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        thisTransform = GetComponent<Transform>();

        flower = GetComponentInChildren<Flower>();
    }
}
