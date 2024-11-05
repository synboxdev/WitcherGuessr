using UnityEngine;
using UnityEngine.EventSystems;

public class SettingTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject TooltipMenu;
    public string SettingName;
    public string SettingDescription;
    private bool isPointerOver = false;

    void Start()
    {
        if (TooltipMenu != null)
            TooltipMenu.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPointerOver)
        {
            isPointerOver = true;
            if (TooltipMenu != null)
            {
                TooltipMenu.SetActive(true);
            }
            TooltipMenu.GetComponent<SettingTooltipMenu>().SettingTooltipName.text = SettingName;
            TooltipMenu.GetComponent<SettingTooltipMenu>().SettingTooltipDescription.text = SettingDescription;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
        {
            isPointerOver = false;
            if (TooltipMenu != null)
            {
                TooltipMenu.SetActive(false);
            }
        }
    }
}