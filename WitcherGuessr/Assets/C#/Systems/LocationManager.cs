using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocationManager : MonoBehaviour
{
    private KeyValuePair<MapType, Location>? CurrentLocation = null;
    private MapType MapTypeForLocations;
    private List<KeyValuePair<MapType, Location>> LocationsQueue;
    private AsyncOperationHandle<Texture> handle;
    private string addressableDownloadStatusText;
    private bool addressableCompleted = false;

    public List<LocationSelection> LocationSelections;

#if UNITY_EDITOR
    public int _fromIndex = -1;
    public int _toIndex = -1;
#endif

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
        CurrentLocation = GetEnqueuedLocation();
        var locationToLoad = CurrentLocation.Value;
        var loadedLocationPanoramicImage = GetLocationCachedImageTexture(locationToLoad);

        if (loadedLocationPanoramicImage is not null)
        {
            RegisterLoadedLocationPanoramicImage(loadedLocationPanoramicImage, locationToLoad);
            DisplayLoadedLocationPanoramicImage(loadedLocationPanoramicImage);
        }
        else
            await LoadAddressableAssetAsync(locationToLoad);
    }

    public async Task PreloadNextLocation()
    {
        var nextLocation = LocationsQueue.FirstOrDefault();

        if (GetLocationCachedImageTexture(nextLocation) is null)
            await LoadAddressableAssetAsync(nextLocation, true);
    }

    public KeyValuePair<MapType, Location>? GetCurrentLocation() => CurrentLocation;

    public bool LocationLoopingTriggered() => LocationsQueue.Count == 0 && !Settings.GetLocationLoopingToggle;

    public void InitializeLocationsQueue(MapType mapType)
    {
        MapTypeForLocations = mapType;
        LocationsQueue = new();

        if (mapType == MapType.AllMaps)
            LocationSelections.ForEach(x => x.LocationsForViewing
                              .ForEach(y => LocationsQueue.Add(new KeyValuePair<MapType, Location>(x.MapType, y))));
        else
            LocationSelections.FirstOrDefault(x => x.MapType == mapType).LocationsForViewing
                              .ForEach(x => LocationsQueue.Add(new KeyValuePair<MapType, Location>(mapType, x)));

#if UNITY_EDITOR
        GetEditorOnlyLocations(mapType);
#endif

        LocationsQueue = LocationsQueue.OrderBy(x => Guid.NewGuid()).ToList();
    }

    void RegisterLoadedLocationPanoramicImage(Texture locationTexture, KeyValuePair<MapType, Location> location) =>
        LocationSelections.FirstOrDefault(x => x.MapType == location.Key).LocationsForViewing
                          .FirstOrDefault(y => y.Index == location.Value.Index)
                          .RegisterAddressableLocationTexture(locationTexture);

    void DisplayLoadedLocationPanoramicImage(Texture locationTexture) =>
        FindObjectOfType<PanoramicImageCameraController>(true).DisplayImage(locationTexture);

    void OnAddressableLocationLoaded(AsyncOperationHandle<Texture> handle, KeyValuePair<MapType, Location> location, bool preloading = false)
    {
        addressableCompleted = true;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            RegisterLoadedLocationPanoramicImage(handle.Result, location);
        else
            Debug.Log("Error has occurred when loading Addressable location from remote directory!");

        if (!preloading)
            DisplayLoadedLocationPanoramicImage(handle.Result);
    }

    async Task LoadAddressableAssetAsync(KeyValuePair<MapType, Location> location, bool preloading = false)
    {
        addressableCompleted = false;
        handle = location.Value.AddressablePanoramicImageTexture.LoadAssetAsync();
        handle.Completed += (handle) => OnAddressableLocationLoaded(handle, location, preloading);
        await handle.Task;
    }

    Texture GetLocationCachedImageTexture(KeyValuePair<MapType, Location> _location)
    {
        var location = LocationSelections.FirstOrDefault(x => x.MapType == _location.Key).LocationsForViewing
                                         .FirstOrDefault(y => y.Index == _location.Value.Index);

        return location.AddressablePanoramicImageTextureLoaded ? location.CachedPanoramicImageTexture : null;
    }

    KeyValuePair<MapType, Location> GetEnqueuedLocation()
    {
        if (LocationsQueue == null || !LocationsQueue.Any())
            InitializeLocationsQueue(MapTypeForLocations);

        var locationToTakeAndRemove = LocationsQueue.FirstOrDefault();
        LocationsQueue.Remove(locationToTakeAndRemove);

        return locationToTakeAndRemove;
    }

#if UNITY_EDITOR
    /// <summary>
    /// Entirely for testing, to quickly see specific locations
    /// </summary>
    void GetEditorOnlyLocations(MapType mapType)
    {
        if (_fromIndex >= 0 && _toIndex >= 0 &&
            LocationSelections.FirstOrDefault(x => x.MapType == mapType) is not null)
        {
            LocationsQueue.Clear();

            LocationSelections
                .FirstOrDefault(x => x.MapType == mapType).LocationsForViewing
                .Where(x => x.Index >= _fromIndex && x.Index <= _toIndex).ToList()
                .ForEach(x => LocationsQueue.Add(new KeyValuePair<MapType, Location>(mapType, x)));
        }
    }
#endif
}