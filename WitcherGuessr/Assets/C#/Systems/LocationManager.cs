using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocationManager : MonoBehaviour
{
    private KeyValuePair<MapType, Location>? CurrentLocation = null;
    private MapType MapTypeForLocations;
    private List<KeyValuePair<MapType, Location>> Locations;
    private AsyncOperationHandle<Texture> handle;
    private string addressableDownloadStatusText;
    private bool addressableCompleted = false;

    public List<LocationSelection> LocationSelections;

#if UNITY_EDITOR
    public int _fromIndex = -1;
    public int _toIndex = -1;
#endif

    void Awake()
    {
        SetInitialConfiguration();
    }

    private void Update()
    {
        if (handle.IsValid() && !handle.IsDone)
        {
            var status = handle.GetDownloadStatus().Percent;
            addressableDownloadStatusText = $"Downloading - {status * 100:0.00}%";
        }
    }

    public string GetLocationDownloadPercentage() => addressableDownloadStatusText;

    public bool GetLocationDownloadStatus() => addressableCompleted;

    public async Task InitializeLocationForViewing()
    {
        CurrentLocation = GetLocation();
        var _CurrentLocation = CurrentLocation.Value.Value;
        var loadedLocationPanoramicImage = GetLoadedLocationSprite(_CurrentLocation.Index);

        if (loadedLocationPanoramicImage is not null)
            RegisterLoadedLocationPanoramicImage(loadedLocationPanoramicImage);
        else
        {
            addressableCompleted = false;
            handle = _CurrentLocation.AddressablePanoramicImageTexture.LoadAssetAsync();
            handle.Completed += OnAddressableLocationLoaded;
            await handle.Task;
        }
    }

    public KeyValuePair<MapType, Location>? GetCurrentLocation()
    {
        return CurrentLocation;
    }

    public void InitializeLocationList(MapType mapType)
    {
        MapTypeForLocations = mapType;
        Locations = new();

        if (mapType == MapType.AllMaps)
            LocationSelections.ForEach(x => x.LocationsForViewing
                              .ForEach(y => Locations.Add(new KeyValuePair<MapType, Location>(x.MapType, y))));
        else
            LocationSelections.FirstOrDefault(x => x.MapType == mapType).LocationsForViewing
                              .ForEach(x => Locations.Add(new KeyValuePair<MapType, Location>(mapType, x)));

        #if UNITY_EDITOR
        GetEditorOnlyLocations(mapType);
        #endif

        Locations = Locations.OrderBy(x => Guid.NewGuid()).ToList();
    }

    void RegisterLoadedLocationPanoramicImage(Texture locationTexture)
    {
        LocationSelections.FirstOrDefault(x => x.MapType == CurrentLocation.Value.Key).LocationsForViewing
                          .FirstOrDefault(y => y.Index == CurrentLocation.Value.Value.Index)
                          .RegisterAddressableLocationTexture(locationTexture);

        FindObjectOfType<PanoramicImageCameraController>(true).DisplayImage(locationTexture);
    }

    void OnAddressableLocationLoaded(AsyncOperationHandle<Texture> handle)
    {
        addressableCompleted = true;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            RegisterLoadedLocationPanoramicImage(handle.Result);
        else
            Debug.Log("Error has occurred when loading Addressable location from remote directory!");
    }

    private Texture GetLoadedLocationSprite(int index)
    {
        var location = LocationSelections.FirstOrDefault(x => x.MapType == CurrentLocation.Value.Key).LocationsForViewing
                                         .FirstOrDefault(y => y.Index == index);

        return location.AddressablePanoramicImageTextureLoaded ? location.CachedPanoramicImageTexture : null;
    }

    private KeyValuePair<MapType, Location> GetLocation()
    {
        if (Locations == null || !Locations.Any())
            InitializeLocationList(MapTypeForLocations);

        var locationToTakeAndRemove = Locations.FirstOrDefault();
        Locations.Remove(locationToTakeAndRemove);

        return locationToTakeAndRemove;
    }

    private void SetInitialConfiguration()
    {
        SetLocationIndexes();
        SetDefaultLocationNames();
    }

    private void SetLocationIndexes()
    {
        foreach (var area in LocationSelections)
            for (int i = 0; i < area.LocationsForViewing.Count; i++)
                area.LocationsForViewing[i].Index = i;
    }

    private void SetDefaultLocationNames()
    {
        foreach (var area in LocationSelections)
            foreach (var locationWithoutName in area.LocationsForViewing.Where(location => string.IsNullOrEmpty(location.Name.Trim())))
                locationWithoutName.Name = area.DefaultLocationName;
    }

    #if UNITY_EDITOR
    /// <summary>
    /// Entirely for testing, to quickly see specific locations
    /// </summary>
    private void GetEditorOnlyLocations(MapType mapType)
    {
        if (_fromIndex >= 0 && _toIndex >= 0 &&
            LocationSelections.FirstOrDefault(x => x.MapType == mapType) is not null)
        {
            Locations.Clear();

            LocationSelections
                .FirstOrDefault(x => x.MapType == mapType).LocationsForViewing
                .Where(x => x.Index >= _fromIndex && x.Index <= _toIndex).ToList()
                .ForEach(x => Locations.Add(new KeyValuePair<MapType, Location>(mapType, x)));
        }
    }
    #endif
}