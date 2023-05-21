using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_MainMenu : MonoBehaviour
{
    public GameObject PlaySelectionMenu;

    public List<MapSelection> MapSelections;
    public int currentMapSelectionIndex;
    public GameObject MapSelectionParentObject;

    public GameObject MapSelectionDefault;
    public GameObject MapSelectionSelected;
    public GameObject MapSelectionSelectedAllMaps;
    public GameObject MapSelectionSelectedBAW;
    public GameObject MapSelectionSelectedHOS;

    void Awake()
    {
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

        foreach (var mapSelection in MapSelections)
        {
            var mapSelectionButton = Instantiate(mapSelection.Index == currentMapSelectionIndex ?
                                                 GetUIMapSelectionPrefab(mapSelection.MapType) : MapSelectionDefault,
                                                 MapSelectionParentObject.transform);
            mapSelectionButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{mapSelection.MapName.ToUpper()}";
            mapSelectionButton.AddComponent<MapSelectionEntity>().Index = (int)mapSelection.MapType;
        }
    }

    public void SwapToGameScene()
    {
        GlobalManager.SetMapSelection(MapSelections.FirstOrDefault(x => x.Index == currentMapSelectionIndex));
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

    private GameObject GetUIMapSelectionPrefab(Map map)
    {
        switch (map)
        {
            case Map.AllMaps:
                return MapSelectionSelectedAllMaps;
            case Map.VelenNovigrad:
            case Map.GauntersWorld:
                return MapSelectionSelectedHOS;
            case Map.Toussaint:
            case Map.ThousandFables:
                return MapSelectionSelectedBAW;
            case Map.Default:
            case Map.WhiteOrchard:
            case Map.SkelligeIsles:
            case Map.KaerMorhen:
            case Map.IsleOfMists:
            default:
                return MapSelectionSelected;
        }
    }
}