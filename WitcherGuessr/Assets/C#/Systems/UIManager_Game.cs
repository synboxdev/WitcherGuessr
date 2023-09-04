using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager_Game : MonoBehaviour
{
    private ResultEvaluationManager ResultEvaluationManager;
    private MapViewCameraMovement MapViewCameraMovement;
    private LocationManager LocationManager;
    private MapManager MapManager;
    private MapMarkerManager MapMarkerManager;

    [SerializeField]
    private MapSelection mapSelection;

    [Header("Map")]
    public GameObject MapParent;
    public TextMeshProUGUI MapName;

    [Header("Location viewing")]
    public GameObject LocationViewUICanvas;
    public GameObject LocationViewInteractiveCanvas;
    public GameObject GuessLocationButton;
    public GameObject GuessLocationSelectionsRect;

    [Header("Location details card")]
    public GameObject LocationDetailsCard;
    public CanvasGroup LocationDetailsCardCanvasGroup;
    public TextMeshProUGUI LocationNameText;
    public TextMeshProUGUI LocationDescriptionText;

    [Header("Map viewing")]
    public GameObject MapViewUICanvas;
    public GameObject MapViewInteractiveCanvas;
    public GameObject ReviewLocationButton;
    public GameObject ConfirmGuessButton;
    public GameObject NextLocationButton;

    [Header("In-game results")]
    public TextMeshProUGUI LocationNumberText;
    public TextMeshProUGUI AverageAccuracyText;
    public TextMeshProUGUI AvailableAttemptsText;

    [Header("End game results")]
    public GameObject EndGameOverlay;
    public Image EndGameBackgroundPanelImage;
    public CanvasGroup EndGameDetailsCanvasGroup;
    public TextMeshProUGUI SuccessfulGuessesText;
    public TextMeshProUGUI AccuracyPercentageText;

    void Awake()
    {
        InitializeInternalSystems();
    }

    void Start()
    {
        mapSelection = GlobalManager.GetMapSelection();
        LocationManager.InitializeLocationList(mapSelection.MapType);
        InitializeUI();
        ConfigureGuessLocationButton();
    }

    public void SwapToMainMenuScene()
    {
        SceneManager.LoadScene((int)SceneIndex.MainMenu);
    }

    public void ReviewLocation()
    {
        MapManager.MapSelections.Where(x => x.MapGameObject != null).ToList().ForEach(map => map.MapGameObject.SetActive(false));
        ToggleViewingCanvas();
    }

    public void ConfirmLocationGuess()
    {
        if (MapMarkerManager.IsUserMarkerPlaced())
        {
            MapMarkerManager.EligibleForUserMarking = false;
            var correctMap = MapManager.MapSelections.FirstOrDefault(map => map.MapType == LocationManager.GetCurrentLocation()?.Key);
            var correctMapWasSelected = ResultEvaluationManager.IsCorrectMapSelected();

            if (!correctMapWasSelected)
                ForceSwapToCorrectMap();

            MapMarkerManager.HandleCorrectLocationDisplay(correctMap.MapGameObject, correctMapWasSelected);

            if (ResultEvaluationManager.EvaluateUserGuess(MapMarkerManager.GetUserMarkerResults()))
                EnableNextLocationUI();

            HandleLocationDetailsCardDisplay();
        }
    }

    public void GoToNextLocation()
    {
        if (!ResultEvaluationManager.GameShouldEnd())
        {
            InitializeLocation();
            ReviewLocation();
            MapMarkerManager.ResetAllMapMarkers();
            ResetDefaultUI();
            ResultEvaluationManager.MovingToNextLocation();
        }
        else
            EnableEndGameUI();
    }

    public void HandleUserGuessResultsToUI(UserGuessResults userGuessResults)
    {
        LocationNumberText.text = $"{userGuessResults.LocationNumber}";
        AverageAccuracyText.text = $"{userGuessResults.UserGuesses.Select(x => x.Accuracy).DefaultIfEmpty(0).Average():0}%";
        AvailableAttemptsText.text = $"{userGuessResults.AvailableAttempts}";
    }

    private void HandleLocationDetailsCardDisplay()
    {
        var correctLocation = LocationManager.GetCurrentLocation()?.Value;
        LocationDetailsCard.SetActive(true);
        LocationNameText.text = $"{correctLocation.Name}";
        LocationDescriptionText.text = $"{correctLocation.Description}";

        LeanTween.value(LocationDetailsCardCanvasGroup.gameObject, 0, 1, 0.5f)
            .setEaseOutQuart()
            .setOnUpdate((float alphaValue) => LocationDetailsCardCanvasGroup.alpha = alphaValue);
    }

    private void ForceSwapToCorrectMap()
    {
        MapManager.MapSelections.Where(x => x.MapGameObject != null).ToList().ForEach(map => map.MapGameObject.SetActive(false));
        SwapToMap((MapType)(LocationManager.GetCurrentLocation()?.Key));
    }

    private void SwapToMap(MapType mapType)
    {
        MapViewCameraMovement.SetMapForViewing(MapManager.MapSelections.FirstOrDefault(map => map.MapType == mapType));
    }

    private void EnableNextLocationUI()
    {
        for (int i = 0; i < NextLocationButton.transform.parent.childCount; i++)
            NextLocationButton.transform.parent.GetChild(i).gameObject.SetActive(false);

        NextLocationButton.SetActive(true);
    }

    private void ResetDefaultUI()
    {
        ConfirmGuessButton.GetComponent<ConfirmGuessButton>().ToggleActive(false);
        ConfirmGuessButton.SetActive(false);
        ReviewLocationButton.SetActive(false);
        LocationDetailsCard.SetActive(false);

        NextLocationButton.SetActive(false);
    }

    private void ToggleViewingCanvas()
    {
        // Location viewing
        LocationViewUICanvas.SetActive(!LocationViewUICanvas.activeInHierarchy);
        LocationViewInteractiveCanvas.SetActive(!LocationViewInteractiveCanvas.activeInHierarchy);
        GuessLocationButton.SetActive(!GuessLocationButton.activeInHierarchy);

        // Map viewing
        MapViewUICanvas.SetActive(!MapViewUICanvas.activeInHierarchy);
        MapViewInteractiveCanvas.SetActive(!MapViewInteractiveCanvas.activeInHierarchy);
        ReviewLocationButton.SetActive(!ReviewLocationButton.activeInHierarchy);
        ConfirmGuessButton.SetActive(!ConfirmGuessButton.activeInHierarchy);
    }

    private void ToggleMapSelectionRect()
    {
        GuessLocationSelectionsRect.SetActive(!GuessLocationSelectionsRect.activeInHierarchy);
    }

    private void InitializeUI()
    {
        MapName.text = mapSelection.MapName.ToUpper();
        InitializeMaps();
        InitializeLocation();
    }

    private void ConfigureGuessLocationButton()
    {
        if (mapSelection.MapType == MapType.AllMaps)
            GuessLocationButton.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                ToggleMapSelectionRect();
            });
        else
            GuessLocationButton.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                SwapToMap(mapSelection.MapType);
                ToggleViewingCanvas();
            });
    }

    private void InitializeMaps()
    {
        if (mapSelection.MapType == MapType.AllMaps)
        {
            var eligibleMaps = MapManager.MapSelections.Where(x => x.MapType != MapType.AllMaps && x.MapPrefab != null).ToList();
            eligibleMaps.ForEach(map => map.MapGameObject = InitializeMap(map));
            InitializeMapSelections(eligibleMaps);
        }
        else
        {
            var mapToInitialize = MapManager.MapSelections.FirstOrDefault(x => x.MapType == mapSelection.MapType);
            mapToInitialize.MapGameObject = InitializeMap(mapToInitialize);
        }
    }

    private void InitializeLocation()
    {
        LocationManager.InitializeLocationForViewing();
    }

    private GameObject InitializeMap(MapSelection mapToInitialize)
    {
        var initializedMap = Instantiate(mapToInitialize.MapPrefab, MapParent.transform);
        initializedMap.SetActive(false);

        return mapToInitialize.MapGameObject = initializedMap;
    }

    private void InitializeMapSelections(List<MapSelection> maps)
    {
        foreach (var map in maps)
        {
            var mapSelectionButton = Instantiate(MapManager.GetUIMapSelectionPrefab(map.MapType), GuessLocationSelectionsRect.transform);
            mapSelectionButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{map.MapName.ToUpper()}";
            mapSelectionButton.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                SwapToMap(map.MapType);
                ToggleViewingCanvas();
                ToggleMapSelectionRect();
            });
        }
    }

    private void EnableEndGameUI()
    {
        EndGameOverlay.SetActive(true);
        SuccessfulGuessesText.text = ResultEvaluationManager.GetSuccessfulGuessesText();
        AccuracyPercentageText.text = ResultEvaluationManager.GetAccuracyPercentageText();

        LeanTween.value(EndGameBackgroundPanelImage.gameObject, 0, 1, 1f)
            .setEaseOutQuart()
            .setOnUpdate((float alphaValue) => EndGameBackgroundPanelImage.color = new Color(EndGameBackgroundPanelImage.color.r,
                                                                                             EndGameBackgroundPanelImage.color.g,
                                                                                             EndGameBackgroundPanelImage.color.b,
                                                                                             alphaValue));

        LeanTween.value(EndGameDetailsCanvasGroup.gameObject, 0, 1, 1f)
            .setEaseOutQuart()
            .setOnUpdate((float alphaValue) => EndGameDetailsCanvasGroup.alpha = alphaValue);
    }

    private void InitializeInternalSystems()
    {
        ResultEvaluationManager = FindObjectOfType<ResultEvaluationManager>();
        MapViewCameraMovement = FindObjectOfType<MapViewCameraMovement>();
        LocationManager = FindObjectOfType<LocationManager>();
        MapManager = FindObjectOfType<MapManager>();
        MapMarkerManager = FindObjectOfType<MapMarkerManager>();
    }
}