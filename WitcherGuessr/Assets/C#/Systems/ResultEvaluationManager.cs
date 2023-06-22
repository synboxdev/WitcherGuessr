using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultEvaluationManager : MonoBehaviour
{
    private MapManager MapManager;
    private LocationManager LocationManager;

    void Awake()
    {
        LocationManager = FindObjectOfType<LocationManager>();
        MapManager = FindObjectOfType<MapManager>();
    }

    public bool IsCorrectMapSelected()
    {
        return LocationManager.GetCurrentLocation()?.Key == MapManager.MapSelections.FirstOrDefault(x => x.IsMarkedByUser).MapType;
    }

    public bool EvaluateUserGuess(KeyValuePair<bool, float> userMarkerResults)
    {
        // TODO: Evalute user guess based on results.
        // If wrong map was selected or distance to center is over 100 - subtract from attempts
        // otherwise - calculate new average for all attemps.
        return true;
    }

    public void RegisterUserGuessResults()
    {

    }
}