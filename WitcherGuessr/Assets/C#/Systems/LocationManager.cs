using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    private ImageInitializationManager ImageInitializationManager = null;
    private Location CurrentLocation = null;

    public List<LocationSelection> LocationSelections;

    void Awake()
    {
        SetLocationIndexes();
    }

    public void InitializeLocationForViewing(MapType? mapType)
    {
        ImageInitializationManager ??= FindObjectOfType<ImageInitializationManager>();

        CurrentLocation = GetLocation(mapType, null);
        ImageInitializationManager.SetNewImage(CurrentLocation.PanoramicImage);
    }

    private Location GetLocation(MapType? mapType, int? locationIndex)
    {
        List<Location> locations = null;

        if (mapType != null &&
            LocationSelections.Any(x => x.MapType == mapType) &&
            LocationSelections.FirstOrDefault(x => x.MapType == mapType).LocationsForViewing.Any())
        {
            locations = LocationSelections.FirstOrDefault(x => x.MapType == mapType).LocationsForViewing;
        }
        else
        {
            var potentialMaps = LocationSelections.Where(x => x.LocationsForViewing.Any()).ToList();
            locations = potentialMaps[Random.Range(0, potentialMaps.Count)].LocationsForViewing;
        }

        if (locationIndex != null)
            return locations[(int)locationIndex];

        var eligibleLocations = CurrentLocation != null && locations.Count > 1 ?
                                locations.Where(l => l.Index != CurrentLocation.Index).ToList() : locations;

        return eligibleLocations[Random.Range(0, eligibleLocations.Count)];
    }

    private void SetLocationIndexes()
    {
        foreach (var area in LocationSelections)
            for (int i = 0; i < area.LocationsForViewing.Count; i++)
                area.LocationsForViewing[i].Index = i;
    }
}