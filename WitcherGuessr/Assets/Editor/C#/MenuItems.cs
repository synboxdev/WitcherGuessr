using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

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
            var LocationManager = FindFirstObjectByType<LocationManager>();
            var MapMarkerManager = FindFirstObjectByType<MapMarkerManager>();
            var MapManager = FindFirstObjectByType<MapManager>();

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
        Debug.Log("Starting location value default value initialization");

        var LocationManager = FindFirstObjectByType<LocationManager>();
        EditorUtility.SetDirty(LocationManager);

        foreach (var area in LocationManager.LocationSelections)
            for (int i = 0; i < area.LocationsForViewing.Count; i++)
                area.LocationsForViewing[i].Index = i;

        foreach (var area in LocationManager.LocationSelections)
        {
            var eligibleLocations = area.LocationsForViewing.Where(location => string.IsNullOrEmpty(location.Name.Trim())).ToList();

            foreach (var locationWithoutName in eligibleLocations)
                locationWithoutName.Name = area.DefaultLocationName;
        }

        foreach (var locations in LocationManager.LocationSelections.Where(x => x.LocationsForViewing.Any()))
        {
            var locationAddressableLabel = $"{Enum.GetName(typeof(MapType), locations.MapType).ToLower()}";
            Addressables.LoadAssetsAsync<Texture>(locationAddressableLabel, null).Completed += handle => OnTexturesLoaded(handle, locations.MapType);
        }

        Debug.Log("Location value default value initialization has finished");
    }

    private static bool isExpanded = true;

    [MenuItem("Custom/Toggle Inspector Items %#e")]
    public static void ToggleInspectorItems()
    {
        foreach (GameObject obj in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            var components = obj.GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                if (component != null)
                {
                    var editor = Editor.CreateEditor(component);
                    if (editor != null)
                    {
                        SerializedObject serializedObject = new SerializedObject(component);
                        SerializedProperty property = serializedObject.GetIterator();
                        while (property.NextVisible(true))
                        {
                            if (property.isArray && property.propertyType != SerializedPropertyType.String)
                            {
                                property.isExpanded = isExpanded;
                                for (int i = 0; i < property.arraySize; i++)
                                {
                                    SerializedProperty element = property.GetArrayElementAtIndex(i);
                                    element.isExpanded = isExpanded;
                                }
                            }
                        }
                        serializedObject.ApplyModifiedProperties();
                        Object.DestroyImmediate(editor);
                    }
                }
            }
        }
        isExpanded = !isExpanded;
        Debug.Log(isExpanded ? "Collapsed all Inspector items." : "Expanded all Inspector items.");
    }

    private static void OnTexturesLoaded(AsyncOperationHandle<IList<Texture>> handle, MapType map)
    {
        var mapName = Enum.GetName(typeof(MapType), map);
        var LocationManager = FindFirstObjectByType<LocationManager>();

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
            Debug.LogError($"Failed to load textures for map: {mapName}");
        }

        if (ValidateMapTextureProperties(textureReferences) &&
            ValidateAssignableLocationProperties(assignableLocations))
        {
            Debug.Log($"[{mapName}] All textures have correct parameters. Initializing their default values!");
            MapTextureReferencesToLocationEntities(textureReferences, assignableLocations);
        }
        else
            Debug.Log($"There has been problems with textures. Fix them!");
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
            case MapType.Fablesphere:
                return "F";
        }

        return null;
    }

    private static bool ValidateMapTextureProperties(List<Tuple<string, AssetReferenceTexture>> textures)
    {
        int invalidTextures = 0;

        foreach (var texture in textures.Select(x => x.Item2))
        {
            var editorAsset = texture.editorAsset;

            if (editorAsset.height != 4096 || editorAsset.width != 8192 || (editorAsset.width / editorAsset.height) != 2f)
            {
                Debug.Log($"Texture by name: '{editorAsset.name}' has invalid width/height proportions.");
                invalidTextures++;
                continue;
            }

            if (editorAsset.filterMode != FilterMode.Trilinear)
            {
                Debug.Log($"Texture by name: {editorAsset.name} has incorrect filter mode. " +
                    $"Expected: [{Enum.GetName(typeof(FilterMode), FilterMode.Trilinear)}], " +
                    $"Current [{Enum.GetName(typeof(FilterMode), editorAsset.filterMode)}]");
                invalidTextures++;
                continue;
            }

            if (editorAsset.wrapMode != TextureWrapMode.Repeat)
            {
                Debug.Log($"Texture by name: {editorAsset.name} has incorrect wrap mode. " +
                    $"Expected: [{Enum.GetName(typeof(TextureWrapMode), TextureWrapMode.Repeat)}], " +
                    $"Current [{Enum.GetName(typeof(TextureWrapMode), editorAsset.wrapMode)}]");
                invalidTextures++;
                continue;
            }
        }

        return invalidTextures == 0;
    }

    private static bool ValidateAssignableLocationProperties(List<Location> locations)
    {
        int invalidLocations = 0;

        foreach (var location in locations)
        {
            if (locations
                .Where(x => x.Index != location.Index)
                .Any(x => x.Coordinates.x == location.Coordinates.x && x.Coordinates.y == location.Coordinates.y))
            {
                var otherLocation = locations
                    .Where(x => x.Index != location.Index)
                    .FirstOrDefault(x => x.Coordinates.x == location.Coordinates.x && x.Coordinates.y == location.Coordinates.y);

                Debug.Log($"Location by index: '{location.Index}' has conflicting coordinates with location by index {otherLocation.Index} invalid width/height proportions.");
                invalidLocations++;
                continue;
            }

            if (string.IsNullOrEmpty(location.Description))
            {
                Debug.Log($"Location by index {location.Index} does NOT have a description written! Write it!");
                invalidLocations++;
                continue;
            }
        }

        return invalidLocations == 0;
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