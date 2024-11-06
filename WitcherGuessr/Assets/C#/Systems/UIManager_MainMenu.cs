using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager_MainMenu : MonoBehaviour
{
    private SettingsManager SettingsManager;
    private MapManager MapManager;
    private LocationManager LocationManager;

    private const string BackgroundVideoName = "MainMenuBackgroundVideo.webm";
    public VideoPlayer VideoPlayer;

    public GameObject PlaySelectionMenu;
    public GameObject AboutSection;
    public GameObject SettingsMenu;
    private List<GameObject> Menus;

    public GameObject MapSelectionParentObject;
    public TextMeshProUGUI ProjectVersionText;
    public TextMeshProUGUI AvailableLocationsText;
    public int currentMapSelectionIndex;

    [Header("Image view sensitivity setting")]
    public Slider ImageViewSensitivitySlider;
    public TextMeshProUGUI ImageViewSensitivitySliderValueText;

    [Header("Map zoom sensitivity setting")]
    public Slider MapZoomSensitivitySlider;
    public TextMeshProUGUI MapZoomSensitivitySliderValueText;

    [Header("Guess attempts variable setting")]
    public Slider GuessAttemptsSlider;
    public TextMeshProUGUI GuessAttemptsSliderValueText;

    [Header("Location looping setting")]
    public Toggle LocationLoopingToggle;

    [Header("Enable hints setting")]
    public Toggle EnableHintsToggle;

    [Header("Location preloading settings")]
    public Toggle LocationPreloadingToggle;

    void Awake()
    {
        SettingsManager = FindFirstObjectByType<SettingsManager>();
        MapManager = FindFirstObjectByType<MapManager>();
        LocationManager = FindFirstObjectByType<LocationManager>();
        ConfigureMainMenuDefaultDisplay();
        InitializeMenus();
    }

    void Start()
    {
        VideoPlayer.url = Path.Combine(Application.streamingAssetsPath, BackgroundVideoName);
    }

    void Update()
    {
        if (SettingsMenu.activeInHierarchy)
            UpdateSettingsUI();
    }

    public void ApplicationQuit() => Application.Quit();

    public void TogglePlaySelectionMenu() => SetActiveMenu(PlaySelectionMenu);

    public void ToggleAboutSection() => SetActiveMenu(AboutSection);

    public void ToggleSettingsMenu() => SetActiveMenu(SettingsMenu);

    public void SpawnMapSelectionButtons()
    {
        DestroyAllMapSelections();

        // Only display map selection buttons of maps who have ANY locations set for them, excluding 'All maps' selection.
        var eligibleMapSelections = MapManager.MapSelections.Where(x => LocationManager.LocationSelections.Any(l => l.MapType == x.MapType && l.LocationsForViewing.Any()) ||
                                                  x.MapType == MapType.AllMaps).ToList();

        foreach (var mapSelection in eligibleMapSelections)
        {
            var mapSelectionButton = Instantiate(mapSelection.Index == currentMapSelectionIndex ?
                                                 MapManager.GetUIMapSelectionPrefab(mapSelection.MapType) : MapManager.MapSelectionDefault,
                                                 MapSelectionParentObject.transform);
            mapSelectionButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{mapSelection.MapName.ToUpper()}";
            var mapSelectionEntity = GetMapSelectionEntity(mapSelectionButton, mapSelection);
        }
    }

    public void SwapToGameScene()
    {
        GlobalManager.SetMapSelection(MapManager.MapSelections.FirstOrDefault(x => x.Index == currentMapSelectionIndex));
        SceneManager.LoadScene((int)SceneIndex.Game);
    }

    private void SetActiveMenu(GameObject menu)
    {
        foreach (var item in Menus.Where(x => x != menu))
            item.SetActive(false);
        
        menu.SetActive(!menu.activeInHierarchy);
    }

    private void DestroyAllMapSelections()
    {
        if (MapSelectionParentObject.transform.childCount > 0)
        {
            foreach (Transform mapSelection in MapSelectionParentObject.transform)
                Destroy(mapSelection.gameObject);
        }
    }

    private void ConfigureMainMenuDefaultDisplay()
    {
        SpawnMapSelectionButtons();
        PlaySelectionMenu.SetActive(false);
        AboutSection.SetActive(false);
        AvailableLocationsText.text = $"Available locations: {GetAvailableLocationsCount(MapType.AllMaps)}";
        ProjectVersionText.text = $"v{Application.version}";
    }

    private void InitializeMenus()
    {
        Menus = new List<GameObject> 
        { 
            PlaySelectionMenu, 
            AboutSection,
            SettingsMenu
        };
    }

    private MapSelectionEntity GetMapSelectionEntity(GameObject mapSelectionButton, MapSelection mapSelection)
    {
        var availableLocations = GetAvailableLocationsCount(mapSelection.MapType);

        var mapSelectionEntity = mapSelectionButton.AddComponent<MapSelectionEntity>();
        mapSelectionEntity.Index = (int)mapSelection.MapType;
        mapSelectionEntity.AvailableLocations = availableLocations;

        mapSelectionEntity.SetAvailableLocationsTextReference(AvailableLocationsText);

        return mapSelectionEntity;
    }

    private int GetAvailableLocationsCount(MapType mapType) => mapType == MapType.AllMaps ?
                                 LocationManager.LocationSelections.Sum(x => x.LocationsForViewing.Count) :
                                 LocationManager.LocationSelections.FirstOrDefault(x => x.MapType == mapType).LocationsForViewing.Count;

    private void UpdateSettingsUI()
    {
        ImageViewSensitivitySliderValueText.text = $"{SettingsManager.GetImageViewSensitivity()}";
        ImageViewSensitivitySlider.value = SettingsManager.GetImageViewSensitivity();

        MapZoomSensitivitySliderValueText.text = $"{SettingsManager.GetMapZoomSensitivity()}";
        MapZoomSensitivitySlider.value = SettingsManager.GetMapZoomSensitivity();

        GuessAttemptsSliderValueText.text = $"{SettingsManager.GetGuessAttempts()}";
        GuessAttemptsSlider.value = SettingsManager.GetGuessAttempts();

        LocationLoopingToggle.SetIsOnWithoutNotify(SettingsManager.GetLocationLoopingToggle());
        EnableHintsToggle.SetIsOnWithoutNotify(SettingsManager.GetEnableHintsToggle());
        LocationPreloadingToggle.SetIsOnWithoutNotify(SettingsManager.GetEnableLocationPreloading());
    }
}