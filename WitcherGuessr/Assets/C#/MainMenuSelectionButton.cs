using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuSelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Color defaultColor;
    private bool isHoveringOver = false;

    void Awake()
    {
        defaultColor = GetComponent<Image>().color;
    }

    void Update()
    {
        if (isHoveringOver)
        {
            var colorAlpha = Mathf.Lerp(.5f, 1f, Mathf.PingPong((Time.time), .75f));
            GetComponent<Image>().color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, colorAlpha);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isHoveringOver = !isHoveringOver;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isHoveringOver = !isHoveringOver;
        GetComponent<Image>().color = defaultColor;
    }
}