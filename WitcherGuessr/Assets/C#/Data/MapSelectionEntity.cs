using UnityEngine;
using UnityEngine.EventSystems;

public class MapSelectionEntity : MonoBehaviour, ISelectHandler
{
    private UIManager_MainMenu UIManager;
    public int Index;

    void Awake()
    {
        UIManager = FindObjectOfType<UIManager_MainMenu>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        UIManager.currentMapSelectionIndex = Index;
        UIManager.SpawnMapSelectionButtons();
    }
}