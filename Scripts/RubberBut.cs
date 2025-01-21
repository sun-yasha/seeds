using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RubberBut : Selectable, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvents.OnClearNumberMethod();
    }
}
