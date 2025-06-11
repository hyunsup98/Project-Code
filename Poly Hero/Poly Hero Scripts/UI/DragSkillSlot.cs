using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSkillSlot : Singleton<DragSkillSlot>
{
    public SkillUISlot dragslot;
    [SerializeField] Image img;

    public void SetItemData(Sprite itemSprite)
    {
        img.sprite = itemSprite;
        Setcolor(1);
        transform.SetAsLastSibling();
    }

    public void Setcolor(float alpha)
    {
        Color color = img.color;
        color.a = alpha;
        img.color = color;
    }
}
