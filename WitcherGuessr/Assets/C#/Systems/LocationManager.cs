using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    private ImageInitializationManager ImageInitializationManager = null;
    private KeyValuePair<MapType, Location>? CurrentLocation = null;
    private MapType MapTypeForLocations;
    private List<KeyValuePair<MapType, Location>> Locations;

    public List<LocationSelection> LocationSelections;

    void Awake()
    {
        SetInitialConfiguration();
    }

    public void InitializeLocationForViewing()
    {
        ImageInitializationManager = FindObjectOfType<ImageInitializationManager>();

        CurrentLocation = GetLocation();
        ImageInitializationManager.SetNewImage(CurrentLocation.Value.Value.PanoramicImage);
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

        Locations = Locations.OrderBy(x => Guid.NewGuid()).ToList();
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
}