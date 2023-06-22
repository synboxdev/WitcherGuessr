using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    private ImageInitializationManager ImageInitializationManager = null;
    private KeyValuePair<MapType, Location>? CurrentLocation = null;

    public List<LocationSelection> LocationSelections;

    void Awake()
    {
        SetLocationIndexes();
    }

    public void InitializeLocationForViewing(MapType? mapType)
    {
        ImageInitializationManager = FindObjectOfType<ImageInitializationManager>();

        CurrentLocation = GetLocation(mapType, null);
        ImageInitializationManager.SetNewImage(CurrentLocation.Value.Value.PanoramicImage);
    }

    public KeyValuePair<MapType, Location>? GetCurrentLocation()
    {
        return CurrentLocation;
    }

    private KeyValuePair<MapType, Location> GetLocation(MapType? mapType, int? locationIndex)
    {
        var chosenMap = GetMap(mapType);
        var chosenLocation = GetLocation(chosenMap, locationIndex);
        return new KeyValuePair<MapType, Location>(chosenMap.MapType, chosenLocation);
    }

    private LocationSelection GetMap(MapType? mapType)
    {
        LocationSelection locationSelection = null;

        if (mapType != null &&
            LocationSelections.Any(x => x.MapType == mapType) &&
            LocationSelections.FirstOrDefault(x => x.MapType == mapType).LocationsForViewing.Any())
        {
            locationSelection = LocationSelections.FirstOrDefault(x => x.MapType == mapType);
        }
        else
        {
            var potentialMaps = LocationSelections.Where(x => x.LocationsForViewing.Any()).ToList();
            locationSelection = potentialMaps.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }

        return locationSelection;
    }

    private Location GetLocation(LocationSelection chosenMap, int? locationIndex)
    {
        if (locationIndex != null)
            return chosenMap.LocationsForViewing[(int)locationIndex];

        return CurrentLocation.HasValue ?
            chosenMap.LocationsForViewing.Where(potentialLocation => potentialLocation.Index != CurrentLocation.Value.Value.Index).ToList().OrderBy(x => Guid.NewGuid()).FirstOrDefault() :
            chosenMap.LocationsForViewing.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
    }

    private void SetLocationIndexes()
    {
        foreach (var area in LocationSelections)
            for (int i = 0; i < area.LocationsForViewing.Count; i++)
                area.LocationsForViewing[i].Index = i;
    }
}