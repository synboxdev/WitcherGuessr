using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager_Game : MonoBehaviour
{
    private MapViewCameraMovement MapViewCameraMovement;
    private MapManager MapManager;
    private MapSelection mapSelection;

    [Header("Map and Markers")]
    public GameObject Map;
    public TextMeshProUGUI MapName;

    [Header("Location viewing")]
    public GameObject LocationViewUICanvas;
    public GameObject LocationViewInteractiveCanvas;
    public GameObject GuessLocationButton;
    public GameObject GuessLocationSelectionsRect;

    [Header("Map viewing")]
    public GameObject MapViewUICanvas;
    public GameObject MapViewInteractiveCanvas;
    public GameObject ReviewLocationButton;
    public GameObject ConfirmGuessButton;

    void Awake()
    {
        MapViewCameraMovement = FindObjectOfType<MapViewCameraMovement>();
        MapManager = FindObjectOfType<MapManager>();
    }

    void Start()
    {
        mapSelection = GlobalManager.GetMapSelection();
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

    private void SwapToMap(MapType mapType)
    {
        var mapToView = MapManager.MapSelections.FirstOrDefault(map => map.MapType == mapType).MapGameObject;
        mapToView.SetActive(true);
        MapViewCameraMovement.SetMapForViewing(mapToView);
        ToggleViewingCanvas();
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
    }

    private void ConfigureGuessLocationButton()
    {
        if (mapSelection.MapType == MapType.AllMaps)
        {
            GuessLocationButton.GetComponent<Button>().onClick.AddListener(delegate () { ToggleMapSelectionRect(); });
        }
        else
            GuessLocationButton.GetComponent<Button>().onClick.AddListener(delegate () { SwapToMap(mapSelection.MapType); });
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

    private GameObject InitializeMap(MapSelection mapToInitialize)
    {
        var initializedMap = Instantiate(mapToInitialize.MapPrefab, Map.transform);
        initializedMap.SetActive(false);

        return mapToInitialize.MapGameObject = initializedMap;
    }

    private void InitializeMapSelections(List<MapSelection> maps)
    {
        foreach (var map in maps)
        {
            var mapSelectionButton = Instantiate(MapManager.GetUIMapSelectionPrefab(map.MapType), GuessLocationSelectionsRect.transform);
            mapSelectionButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{map.MapName.ToUpper()}";
            mapSelectionButton.GetComponent<Button>().onClick.AddListener(delegate () { SwapToMap(map.MapType); ToggleMapSelectionRect(); });
        }
    }
}