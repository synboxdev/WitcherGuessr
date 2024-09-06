using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MenuItems : MonoBehaviour
{
    private static GameObject allLocationsHolder = null;

    /// <summary>
    /// Force spawns all location markers for currently opened map.
    /// For some reason LINQ breaks inside Editor scripts, so some manual loops are being used here.
    /// </summary>
    [MenuItem("Custom/Spawn all locations of current map")]
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


    [MenuItem("Custom/Initialize location default values")]
    [ExecuteInEditMode]
    private static void InitializeLocationIndexes()
    {
        var LocationManager = FindObjectOfType<LocationManager>();

        foreach (var area in LocationManager.LocationSelections)
            for (int i = 0; i < area.LocationsForViewing.Count; i++)
                area.LocationsForViewing[i].Index = i;

        foreach (var area in LocationManager.LocationSelections)
            foreach (var locationWithoutName in area.LocationsForViewing.Where(location => string.IsNullOrEmpty(location.Name.Trim())))
                locationWithoutName.Name = area.DefaultLocationName;

        foreach (var locations in LocationManager.LocationSelections.Where(x => x.LocationsForViewing.Any()))
        {
            var locationAddressableLabel = $"{Enum.GetName(typeof(MapType), locations.MapType).ToLower()}";
            Addressables.LoadAssetsAsync<Texture>(locationAddressableLabel, null).Completed += handle =>
                OnTexturesLoaded(handle, locations.MapType);
        }
    }

    private static void OnTexturesLoaded(AsyncOperationHandle<IList<Texture>> handle, MapType map)
    {
        var LocationManager = FindObjectOfType<LocationManager>();

        var locationNamePrefix = GetLocationNamePrefixByMapType(map);
        var assignableLocations = LocationManager.LocationSelections.FirstOrDefault(x => x.MapType == map).LocationsForViewing;
        var textureReferences = new List<Tuple<string, AssetReferenceTexture>>();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var texture in handle.Result)
            {
                string assetGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(texture));
                var textureReference = new AssetReferenceTexture(assetGUID);
                var textureName = textureReference.editorAsset.name;
                var textureTuple = new Tuple<string, AssetReferenceTexture>(textureName, textureReference);
                textureReferences.Add(textureTuple);
            }
        }
        else
        {
            Debug.LogError($"Failed to load textures for map: {Enum.GetName(typeof(MapType), map)}");
        }

        MapTextureReferencesToLocationEntities(textureReferences, assignableLocations);
    }

    private static string GetLocationNamePrefixByMapType(MapType mapType)
    {
        switch (mapType)
        {
            case MapType.WhiteOrchard:
                return "W";
            case MapType.VelenNovigrad:
                return "V";
            case MapType.SkelligeIsles:
                return "S";
            case MapType.KaerMorhen:
                return "K";
            case MapType.IsleOfMists:
                return "I";
            case MapType.GauntersWorld:
                return "G";
            case MapType.Toussaint:
                return "T";
            case MapType.Fables:
                return "F";
        }

        return null;
    }

    private static void MapTextureReferencesToLocationEntities(
        List<Tuple<string, AssetReferenceTexture>> references, 
        List<Location> locations)
    {
        var formattedReferences = new List<Tuple<int, AssetReferenceTexture>>();

        foreach (var reference in references)
        {
            int.TryParse(Regex.Replace(reference.Item1, "[^0-9]", ""), out int index);
            formattedReferences.Add(new Tuple<int, AssetReferenceTexture>(index, reference.Item2));
        }

        foreach (var location in locations)
            location.AddressablePanoramicImageTexture = formattedReferences.FirstOrDefault(x => x.Item1 == location.Index).Item2;
    }
}