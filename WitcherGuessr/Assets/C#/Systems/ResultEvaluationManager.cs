using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultEvaluationManager : MonoBehaviour
{
    private MapManager MapManager;
    private LocationManager LocationManager;
    private UIManager_Game UIManager;

    private UserGuessResults UserGuessResults;

    void Awake()
    {
        LocationManager = FindObjectOfType<LocationManager>();
        MapManager = FindObjectOfType<MapManager>();
        UIManager = FindObjectOfType<UIManager_Game>();
    }

    void Start()
    {
        UserGuessResults = new UserGuessResults();
        UIManager.HandleUserGuessResultsToUI(UserGuessResults);
    }

    public string GetSuccessfulGuessesText()
    {
        return $"{UserGuessResults.UserGuesses.Count(x => x.IsAccurate)}/{UserGuessResults.UserGuesses.Count}";
    }

    public string GetAccuracyPercentageText()
    {
        return $"{UserGuessResults.UserGuesses.Select(x => x.Accuracy).DefaultIfEmpty(0).Average():0}%";
    }

    public bool GameShouldEnd()
    {
        return UserGuessResults.AvailableAttempts <= 0;
    }

    public bool IsCorrectMapSelected()
    {
        return LocationManager.GetCurrentLocation()?.Key == MapManager.MapSelections.FirstOrDefault(x => x.IsMarkedByUser).MapType;
    }

    public void MovingToNextLocation()
    {
        UserGuessResults.LocationNumber++;
        UIManager.HandleUserGuessResultsToUI(UserGuessResults);
    }

    public bool EvaluateUserGuess(KeyValuePair<bool, float> userMarkerResults)
    {
        RegisterUserGuessResults(userMarkerResults);
        UIManager.HandleUserGuessResultsToUI(UserGuessResults);
        return UserGuessResults.AvailableAttempts >= 0;
    }

    private void RegisterUserGuessResults(KeyValuePair<bool, float> userMarkerResults)
    {
        if (!userMarkerResults.Key || userMarkerResults.Value > 100)
        {
            UserGuessResults.AvailableAttempts--;
            UserGuessResults.UserGuesses.Add(new() { IsAccurate = false, Accuracy = 0 });
        }
        else
            UserGuessResults.UserGuesses.Add(new() { IsAccurate = true, Accuracy = userMarkerResults.Value });
    }
}