using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuItems : MonoBehaviour
{
    private static GameObject allLocationsHolder = null;

    /// <summary>
    /// Force spawns all location markers for currently opened map.
    /// For some reason LINQ breaks inside Editor scripts, so some manual loops are being used here.
    /// </summary>
    [MenuItem("Custom/Spawn All Locations Of Current Map")]
    private static void SpawnAllLocationsOfCurrentMap()
    {
        if (allLocationsHolder == null)
        {
            var LocationManager = FindObjectOfType<LocationManager>();
            var MapMarkerManager = FindObjectOfType<MapMarkerManager>();
            var MapManager = FindObjectOfType<MapManager>();

            MapSelection _currentMapSelection = null;
            foreach (var map in MapManager.MapSelections)
            {
                if (map.MapGameObject != null && map.MapGameObject.activeInHierarchy)
                {
                    _currentMapSelection = map;
                    break;
                }
            }

            var _allLocationsOfCurrentMap = new List<Location>();
            foreach (var location in LocationManager.LocationSelections)
            {
                if (location.MapType == _currentMapSelection.MapType)
                {
                    _allLocationsOfCurrentMap = location.LocationsForViewing;
                    break;
                }
            }

            var gameObjectOfCurrentMap = _currentMapSelection.MapGameObject;
            allLocationsHolder = new GameObject("_AllLocationHolder");
            allLocationsHolder.transform.SetParent(gameObjectOfCurrentMap.transform);

            foreach (var location in _allLocationsOfCurrentMap)
            {
                var _location = Instantiate(MapMarkerManager.LocationMarkerPrefab, allLocationsHolder.transform);
                _location.GetComponent<SpriteRenderer>().color = Color.cyan;
                _location.name = $"[{location.Index}] - [{location.Coordinates}]";
                _location.transform.position = new Vector3(location.Coordinates.x, location.Coordinates.y, -1f);
            }
        }
        else
            Destroy(allLocationsHolder);
    }
}