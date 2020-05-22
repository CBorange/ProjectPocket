using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchStateCatcher : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        UITouchStateContainer.Instance.PossibleToControll = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UITouchStateContainer.Instance.PossibleToControll = true;
    }
}
