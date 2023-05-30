using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_MainMenu : MonoBehaviour
{
    private MapManager MapManager;

    public GameObject PlaySelectionMenu;
    public GameObject MapSelectionParentObject;
    public int currentMapSelectionIndex;

    void Awake()
    {
        MapManager = FindObjectOfType<MapManager>();
        SpawnMapSelectionButtons();
        PlaySelectionMenu.SetActive(false);
    }

    public void TogglePlaySelectionMenu()
    {
        PlaySelectionMenu.SetActive(!PlaySelectionMenu.activeInHierarchy);
    }

    public void SpawnMapSelectionButtons()
    {
        DestroyAllMapSelections();

        foreach (var mapSelection in MapManager.MapSelections)
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

    private void DestroyAllMapSelections()
    {
        if (MapSelectionParentObject.transform.childCount > 0)
        {
            foreach (Transform mapSelection in MapSelectionParentObject.transform)
                Destroy(mapSelection.gameObject);
        }
    }
}