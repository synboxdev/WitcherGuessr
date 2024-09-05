using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public class MapSelection
{
    public int Index;
    public MapType MapType;
    public string MapName;
    public GameObject MapGameObject;
    public GameObject MapPrefab;
    public AssetReferenceSprite AddressableMapSprite;
    public bool AddressableMapLoaded = false;
    public bool IsMarkedByUser = false;
}