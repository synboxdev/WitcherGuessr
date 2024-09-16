using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapSelectionEntity : MonoBehaviour, ISelectHandler
{
    private UIManager_MainMenu UIManager;
    private TextMeshProUGUI AvailableLocationsText;
    public int Index;
    public int AvailableLocations;

    void Awake()
    {
        UIManager = FindObjectOfType<UIManager_MainMenu>();
    }

    public void SetAvailableLocationsTextReference(TextMeshProUGUI availableLocationsText) => AvailableLocationsText = availableLocationsText;

    public void OnSelect(BaseEventData eventData)
    {
        UIManager.currentMapSelectionIndex = Index;
        UIManager.SpawnMapSelectionButtons();
        AvailableLocationsText.text = $"Available locations: {AvailableLocations}";
    }
}