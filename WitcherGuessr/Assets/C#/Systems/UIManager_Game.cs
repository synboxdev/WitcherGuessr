using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager_Game : MonoBehaviour
{
    private ResultEvaluationManager ResultEvaluationManager;
    private MapViewCameraController MapViewCameraController;
    private PanoramicImageCameraController PanoramicImageCameraController;
    private LocationManager LocationManager;
    private MapManager MapManager;
    private MapMarkerManager MapMarkerManager;

    [SerializeField]
    private MapSelection mapSelection;

    [Header("Cameras")]
    public GameObject MapViewCamera;
    public GameObject ImageViewCamera;

    [Header("Parent canvas")]
    public CanvasGroup ParentUICanvas;

    [Header("Map")]
    public TextMeshProUGUI MapName;

    [Header("Location viewing")]
    public GameObject LocationViewUICanvas;
    public GameObject ImageViewSphere;
    public GameObject GuessLocationButton;
    public GameObject GuessLocationSelectionsRect;

    [Header("Display hint")]
    public GameObject DisplayHintButton;

    [Header("Location details card")]
    public GameObject LocationDetailsCard;
    public CanvasGroup LocationDetailsCardCanvasGroup;
    public TextMeshProUGUI LocationNameText;
    public TextMeshProUGUI LocationDescriptionText;
    public TextMeshProUGUI LocationDownloadStatusText;

    [Header("Map viewing")]
    public GameObject MapViewUICanvas;
    public GameObject MapViewInteractiveCanvas;
    public GameObject ReviewLocationButton;
    public GameObject ConfirmGuessButton;
    public GameObject NextLocationButton;
    public TextMeshProUGUI MapDownloadStatusText;

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
    public TextMeshProUGUI HintsUsedText;

    void Awake()
    {
        InitializeInternalSystems();
        ConfigureUI();
    }

    async void Start()
    {
        mapSelection = GlobalManager.GetMapSelection();
        LocationManager.InitializeLocationsQueue(mapSelection.MapType);
        await InitializeUIAsync();
        ConfigureGuessLocationButton();
    }

    private void Update() => HandleUpdatedUI();

    public void SwapToMainMenuScene()
    {
        GameSceneCleanup();
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

            if (ResultEvaluationManager.EvaluateUserGuess(MapMarkerManager.GetUserMarkerResults(), ResultEvaluationManager.LocationIsPreventableWrongGuess))
                EnableNextLocationUI();

            HandleLocationDetailsCardDisplay();
        }
    }

    public async void GoToNextLocation()
    {
        if (!ResultEvaluationManager.GameShouldEnd())
        {
            await InitializeLocationAsync();
            ReviewLocation();
            MapMarkerManager.ResetAllMapMarkers();
            ResetDefaultUI();
            ResultEvaluationManager.MovingToNextLocation();
            
            if (Settings.GetEnableLocationPreloading)
                await PreloadNextLocationAsync();
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

    public void TriggerLocationDetailsCardDisplay()
    {
        DisplayHintButton.SetActive(false);
        ResultEvaluationManager.RegisterHintUsed();
        HandleLocationDetailsCardDisplay();
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
        MapViewCameraController.SetMapForViewing(MapManager.MapSelections.FirstOrDefault(map => map.MapType == mapType));
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
        DisplayHintButton.SetActive(Settings.GetEnableHintsToggle);
        LocationDetailsCard.SetActive(false);
        NextLocationButton.SetActive(false);
        LocationDownloadStatusText.gameObject.SetActive(false);
        MapDownloadStatusText.gameObject.SetActive(false);
    }

    private void ToggleViewingCanvas()
    {
        // Location viewing
        LocationViewUICanvas.SetActive(!LocationViewUICanvas.activeInHierarchy);
        GuessLocationButton.SetActive(!GuessLocationButton.activeInHierarchy);
        ImageViewCamera.SetActive(!ImageViewCamera.activeInHierarchy);

        if (Settings.GetEnableHintsToggle)
            DisplayHintButton.SetActive(!DisplayHintButton.activeInHierarchy);
        
        // Map viewing
        MapViewUICanvas.SetActive(!MapViewUICanvas.activeInHierarchy);
        MapViewInteractiveCanvas.SetActive(!MapViewInteractiveCanvas.activeInHierarchy);
        ReviewLocationButton.SetActive(!ReviewLocationButton.activeInHierarchy);
        ConfirmGuessButton.SetActive(!ConfirmGuessButton.activeInHierarchy);
        MapViewCamera.SetActive(!MapViewCamera.activeInHierarchy);
        ImageViewSphere.SetActive(!ImageViewSphere.activeInHierarchy);
    }

    private void ToggleMapSelectionRect()
    {
        GuessLocationSelectionsRect.SetActive(!GuessLocationSelectionsRect.activeInHierarchy);
    }

    private async Task InitializeUIAsync()
    {
        MapName.text = mapSelection.MapName.ToUpper();
        await InitializeLocationAsync();
        await InitializeMapsAsync();

        if (Settings.GetEnableLocationPreloading)
            await PreloadNextLocationAsync();
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

    private async Task InitializeMapsAsync()
    {
        if (mapSelection.MapType == MapType.AllMaps)
        {
            var eligibleMaps = MapManager.MapSelections
                .Join(LocationManager.LocationSelections,
                      mapSelection => mapSelection.MapType,
                      locationSelection => locationSelection.MapType,
                      (mapSelection, locationSelection) => new { mapSelection, locationSelection })
                .Where(x => x.mapSelection.MapType != MapType.AllMaps)
                .Where(x => x.locationSelection.LocationsForViewing.Any())
                .ToList();

            foreach (var map in eligibleMaps)
                map.mapSelection.MapGameObject = await MapManager.GetInitializedMapAsync(map.mapSelection);
            
            InitializeMapSelections(eligibleMaps.Select(x => x.mapSelection).ToList());
        }
        else
        {
            var mapToInitialize = MapManager.MapSelections.FirstOrDefault(x => x.MapType == mapSelection.MapType);
            mapToInitialize.MapGameObject = await MapManager.GetInitializedMapAsync(mapToInitialize);
        }
    }

    private async Task InitializeLocationAsync() => await LocationManager.InitializeLocationForViewing();

    private async Task PreloadNextLocationAsync() => await LocationManager.PreloadNextLocation();

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
        HintsUsedText.text = ResultEvaluationManager.GetHintsUsedText();

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

    private void GameSceneCleanup()
    {
        MapMarkerManager.DestroyUserMarker();
        MapManager.DisableAllMaps();
        MapMarkerManager.ResetAllMapMarkers();
    }

    private void ConfigureUI()
    {
        if (Settings.GetEnableHintsToggle)
            DisplayHintButton.SetActive(true);
    }

    private void InitializeInternalSystems()
    {
        ResultEvaluationManager = FindObjectOfType<ResultEvaluationManager>();
        MapViewCameraController = FindObjectOfType<MapViewCameraController>();
        PanoramicImageCameraController = FindObjectOfType<PanoramicImageCameraController>();
        LocationManager = FindObjectOfType<LocationManager>();
        MapManager = FindObjectOfType<MapManager>();
        MapMarkerManager = FindObjectOfType<MapMarkerManager>();
    }

    private void HandleUpdatedUI()
    {
        if (!LocationManager.GetLocationDownloadStatus())
        {
            LocationDownloadStatusText.gameObject.SetActive(true);
            LocationDownloadStatusText.text = LocationManager.GetLocationDownloadPercentage();
        }
        else
            LocationDownloadStatusText.gameObject.SetActive(false);
        
        
        if (!MapManager.GetMapDownloadStatus())
        {
            MapDownloadStatusText.gameObject.SetActive(true);
            MapDownloadStatusText.text = MapManager.GetMapDownloadPercentage();
        }
        else
            MapDownloadStatusText.gameObject.SetActive(false);
    }
}