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

    public Texture CachedPanoramicImageTexture = null;
    public AssetReferenceTexture AddressablePanoramicImageTexture;
    public bool AddressablePanoramicImageTextureLoaded = false;

    public void RegisterAddressableLocationTexture(Texture loadedTexture)
    {
        AddressablePanoramicImageTextureLoaded = true;
        CachedPanoramicImageTexture = loadedTexture;
    }
}