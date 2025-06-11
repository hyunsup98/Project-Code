using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBarDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public Transform Inventory;

    Vector2 offset = Vector2.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            offset.x = Inventory.position.x - eventData.position.x;
            offset.y = Inventory.position.y - eventData.position.y;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.position = new Vector2(eventData.position.x + offset.x, eventData.position.y + offset.y);
        }
    }
}
