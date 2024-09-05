using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UIManager_MainMenu : MonoBehaviour
{
    private MapManager MapManager;
    private LocationManager LocationManager;
    private const string BackgroundVideoName = "MainMenuBackgroundVideo.mp4";

    public VideoPlayer VideoPlayer;
    public GameObject PlaySelectionMenu;
    public GameObject AboutSection;
    public GameObject MapSelectionParentObject;
    public TextMeshProUGUI ProjectVersionText;
    public int currentMapSelectionIndex;

    void Awake()
    {
        MapManager = FindObjectOfType<MapManager>();
        LocationManager = FindObjectOfType<LocationManager>();
        ConfigureMainMenuDefaultDisplay();
    }

    void Start()
    {
        VideoPlayer.url = Path.Combine(Application.streamingAssetsPath, BackgroundVideoName);
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    }

    public void TogglePlaySelectionMenu()
    {
        ToggleCloseAllMenus();
        PlaySelectionMenu.SetActive(!PlaySelectionMenu.activeInHierarchy);
    }

    public void ToggleAboutSection()
    {
        ToggleCloseAllMenus();
        AboutSection.SetActive(!PlaySelectionMenu.activeInHierarchy);
    }

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
            mapSelectionButton.AddComponent<MapSelectionEntity>().Index = (int)mapSelection.MapType;
        }
    }

    public void SwapToGameScene()
    {
        GlobalManager.SetMapSelection(MapManager.MapSelections.FirstOrDefault(x => x.Index == currentMapSelectionIndex));
        SceneManager.LoadScene((int)SceneIndex.Game);
    }

    private void ToggleCloseAllMenus()
    {
        PlaySelectionMenu.SetActive(false);
        AboutSection.SetActive(false);
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
        ProjectVersionText.text = $"v{Application.version}";
    }
}