using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmGuessButton : MonoBehaviour
{
    [SerializeField]
    public Color DisabledColor, EnabledColor;

    public void ToggleActive(bool active)
    {
        GetComponent<Button>().interactable = !GetComponent<Button>().interactable;
        GetComponentInChildren<TextMeshProUGUI>().color = active ? EnabledColor : DisabledColor;
    }
}
