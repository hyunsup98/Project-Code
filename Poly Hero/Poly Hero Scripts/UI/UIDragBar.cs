using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] private Transform uiObject;

    Vector2 offset = Vector2.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            offset.x = uiObject.position.x - eventData.position.x;
            offset.y = uiObject.position.y - eventData.position.y;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            uiObject.position = new Vector2(eventData.position.x + offset.x, eventData.position.y + offset.y);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        uiObject.SetAsLastSibling();
        UIManager.Instance.siblingObject = uiObject.gameObject;
    }
}
