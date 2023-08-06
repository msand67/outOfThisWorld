using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;

   /* private void OnMouseEnter()
    {
        ToolTipManager._instance.DisplayTooltip(message);

    }
    private void OnMouseExit()
    {
        ToolTipManager._instance.HideTooltip();
    }*/
    public void OnPointerEnter(PointerEventData pointerEventData)

    {

        ToolTipManager._instance.DisplayTooltip(message);

    }



    public void OnPointerExit(PointerEventData pointerEventData)

    {

        ToolTipManager._instance.HideTooltip();

    }

}
