using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class Location
{
    public int Index;
    public Vector2 Coordinates;
    public string Name;
    public string Description;
    public Sprite PanoramicImage;
    public Sprite CachedPanoramicSprite = null;
    public AssetReferenceSprite AddressablePanoramicSprite;
    public bool AddressablePanoramicSpriteLoaded = false;

    public void RegisterAddressableLocationSprite(Sprite loadedSprite)
    {
        AddressablePanoramicSpriteLoaded = true;
        CachedPanoramicSprite = loadedSprite;
    }
}