using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager_Game : MonoBehaviour
{
    public TextMeshProUGUI MapName;

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

    private void InitializeUI()
    {
        MapName.text = mapSelection.MapName.ToUpper();
    }
}