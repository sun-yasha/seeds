using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NumberButtons : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public int value = 0;//значение кнопки

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData) //точечный щелчок мыши
    {//выполнение события обновления
        GameEvents.UpdateSquareNumberMethod(value);
    }

    void ISubmitHandler.OnSubmit(BaseEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
