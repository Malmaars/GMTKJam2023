using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SoilTile : Interactable
{
    public Flower cropType;
    [Range(0, 3)] public int growthStage;

    private Sprite[] sprites;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void OnInteract()
    {
        // 
    }
}
