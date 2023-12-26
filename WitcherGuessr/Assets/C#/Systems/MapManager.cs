using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private const string _mapParent = "MapParent";
    private GameObject MapParent;

    public List<MapSelection> MapSelections;

    public GameObject MapSelectionDefault;
    public GameObject MapSelectionSelected;
    public GameObject MapSelectionSelectedAllMaps;
    public GameObject MapSelectionSelectedBAW;
    public GameObject MapSelectionSelectedHOS;

    void Awake()
    {
        if (GameObject.FindGameObjectWithTag(_mapParent) is null)
            InitializeMapParent();
    }

    private void InitializeMapParent()
    {
        MapParent = new GameObject()
        {
            name = _mapParent,
            tag = _mapParent
        };
        DontDestroyOnLoad(MapParent);
    }

    public GameObject GetUIMapSelectionPrefab(MapType map)
    {
        switch (map)
        {
            case MapType.AllMaps:
                return MapSelectionSelectedAllMaps;
            case MapType.VelenNovigrad:
            case MapType.GauntersWorld:
                return MapSelectionSelectedHOS;
            case MapType.Toussaint:
            case MapType.ThousandFables:
                return MapSelectionSelectedBAW;
            case MapType.Default:
            case MapType.WhiteOrchard:
            case MapType.SkelligeIsles:
            case MapType.KaerMorhen:
            case MapType.IsleOfMists:
            default:
                return MapSelectionSelected;
        }
    }

    public void DisableAllMaps()
    {
        for (int i = 0; i < MapParent.transform.childCount; i++)
            MapParent.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void MapMarkedByUser(MapSelection mapSelection)
    {
        MapSelections.ForEach(x => x.IsMarkedByUser = false);
        MapSelections.FirstOrDefault(x => x.Index == mapSelection.Index).IsMarkedByUser = true;
    }

#nullable enable
    public GameObject? GetLoadedMapGameObject(MapType mapType)
    {
        return MapSelections.Any(x => x.MapType == mapType && x.AddressableMapLoaded) ?
               MapSelections.FirstOrDefault(x => x.MapType == mapType && x.AddressableMapLoaded).MapGameObject : null;
    }
#nullable disable

    public void RegisterLoadedMapGameObject(MapType mapType, GameObject loadedMapGameObject)
    {
        var mapSelection = MapSelections.FirstOrDefault(x => x.MapType == mapType);
        mapSelection.MapGameObject = loadedMapGameObject;
        mapSelection.AddressableMapLoaded = true;
        loadedMapGameObject.transform.SetParent(MapParent.transform);
    }
}