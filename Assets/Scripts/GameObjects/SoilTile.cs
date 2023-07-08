using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SoilTile : Interactable
{
    public CropType cropType;
    [Range(0, 3)] public int growthStage;

    private Sprite[] sprites;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void AssignTo(CropType type, int stage)
    {
        cropType = type;
        growthStage = stage;
        
        sprites = CropLibrary.instance.GetSprites(type);

        if (growthStage + 1 <= sprites.Length)
        {
            sr.sprite = sprites[growthStage + 1];
        }
    }

    public override void OnInteract()
    {
        // 
    }
}
