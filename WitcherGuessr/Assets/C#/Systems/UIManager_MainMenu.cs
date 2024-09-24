using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager_MainMenu : MonoBehaviour
{
    private MapManager MapManager;
    private LocationManager LocationManager;

    private const string BackgroundVideoName = "MainMenuBackgroundVideo.mp4";
    public VideoPlayer VideoPlayer;

    public GameObject PlaySelectionMenu;
    public GameObject AboutSection;
    private List<GameObject> Menus;

    public GameObject MapSelectionParentObject;
    public TextMeshProUGUI ProjectVersionText;
    public TextMeshProUGUI AvailableLocationsText;
    public int currentMapSelectionIndex;

    void Awake()
    {
        MapManager = FindObjectOfType<MapManager>();
        LocationManager = FindObjectOfType<LocationManager>();
        ConfigureMainMenuDefaultDisplay();
        InitializeMenus();
    }

    void Start()
    {
        VideoPlayer.url = Path.Combine(Application.streamingAssetsPath, BackgroundVideoName);
    }

    public void ApplicationQuit() => Application.Quit();

    public void TogglePlaySelectionMenu() => SetActiveMenu(PlaySelectionMenu);

    public void ToggleAboutSection() => SetActiveMenu(AboutSection);

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
            AboutSection 
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
}