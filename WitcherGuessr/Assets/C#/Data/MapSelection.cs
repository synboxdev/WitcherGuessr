using System;
using UnityEngine;

[Serializable]
public class MapSelection
{
    public MapType MapType;
    public string MapName;
    public int Index;
    public GameObject MapPrefab;
    public GameObject MapGameObject;
    public bool IsMarkedByUser = false;
}