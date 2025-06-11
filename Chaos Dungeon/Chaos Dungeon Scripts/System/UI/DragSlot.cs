using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//IPointClickHandler �������̽� ���� ���̾��Ű ������ ���� ���̴� ������ �������⿡ ���� �巡�� ������ �����
public class DragSlot : MonoBehaviour
{
    //���� �巡�� ���� ���� ���� ����
    public SlotInfo slotinfo;
    //������ ����
    public Item item;
    //������ �̹���
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
