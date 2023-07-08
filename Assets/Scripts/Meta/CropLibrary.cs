using System;
using System.Linq;
using UnityEngine;

public enum CropType
{
    Weeds, Rose, Lavender, 
}

public class CropLibrary : MonoBehaviour
{
    public static CropLibrary instance;
    public CropSpriteSet[] cropSpriteSets;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public Sprite[] GetSprites(CropType cropType)
    {
        CropSpriteSet css = cropSpriteSets.FirstOrDefault(set => set.cropType == cropType);

        if (css != null)
        {
            return css.sprites;
        }
        else
        {
            Debug.LogError("No crop sprites registered for crop type [" + cropType + "]");
            return null;
        }
    }
}

[Serializable]
public class CropSpriteSet
{
    public CropType cropType;
    public Sprite[] sprites;
}