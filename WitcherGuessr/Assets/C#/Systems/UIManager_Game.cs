using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Game : MonoBehaviour
{
    public TextMeshProUGUI MapName;

    // Location viewing
    public GameObject LocationViewCanvas;
    public GameObject GuessLocationButton;

    // Map viewing
    public GameObject MapViewCanvas;
    public GameObject ReviewLocationButton;
    public GameObject ConfirmGuessButton;

    private MapSelection mapSelection;

    void Start()
    {
        mapSelection = GlobalManager.GetMapSelection();
        InitializeUI();
    }

    public void SwapToMainMenuScene()
    {
        SceneManager.LoadScene((int)SceneIndex.MainMenu);
    }

    public void ToggleViewingCanvas()
    {
        LocationViewCanvas.SetActive(!LocationViewCanvas.activeInHierarchy);
        GuessLocationButton.SetActive(!GuessLocationButton.activeInHierarchy);
        MapViewCanvas.SetActive(!MapViewCanvas.activeInHierarchy);
        ReviewLocationButton.SetActive(!ReviewLocationButton.activeInHierarchy);
        ConfirmGuessButton.SetActive(!ConfirmGuessButton.activeInHierarchy);
    }

    private void InitializeUI()
    {
        MapName.text = mapSelection.MapName.ToUpper();
    }
}