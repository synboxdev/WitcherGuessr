using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<MapSelection> MapSelections;

    public GameObject MapSelectionDefault;
    public GameObject MapSelectionSelected;
    public GameObject MapSelectionSelectedAllMaps;
    public GameObject MapSelectionSelectedBAW;
    public GameObject MapSelectionSelectedHOS;

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
}