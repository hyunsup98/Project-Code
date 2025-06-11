using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//IPointClickHandler 인터페이스 사용시 하이어라키 순서에 따라 보이는 순서가 정해지기에 따로 드래그 슬롯을 만든다
public class DragSlot : MonoBehaviour
{
    //현재 드래그 중인 템의 원본 슬롯
    public SlotInfo slotinfo;
    //아이템 정보
    public Item item;
    //아이템 이미지
    public Image img;

    public void SetItem(Item _item)
    {
        item = _item;
        img.sprite = item.render.sprite;
        SetColor(1);
    }

    public void DragItemOff()
    {
        SetColor(0);
        item = null;
    }
    
    void SetColor(float a)
    {
        Color color = img.color;
        color.a = a;
        img.color = color;
    }
}
