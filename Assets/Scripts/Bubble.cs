using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bubble : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool overBubble;

    public void OnPointerEnter(PointerEventData eventData)
    {
        overBubble = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        overBubble = false;
    }
}
