using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotInfo : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Inventory inven;
    [SerializeField] DragSlot dragSlot;

    [SerializeField] GraphicRaycaster gr;

    //������ ������ �����ִ� UIâ
    public GameObject itemInfoUI;
    RectTransform rect;

    public Item item;
    //���Կ� ���� �������� Weapon�̸� weapItem�� ����
    public Weapon weapItem;

    public Image img_item;

    public SlotType slotType;

    // Start is called before the first frame update
    void Start()
    {
        img_item = GetComponent<Image>();
        gr = UIManager.Instance.gr;
        rect = itemInfoUI.GetComponent<RectTransform>();
    }

    //�κ��丮�� ������ �̹��� ǥ��, a�� ���İ�
    public void SetSlot(Item _item)
    {
        float a = 0;
        item = _item;

        if (item != null)
        {
            img_item.sprite = item.render.sprite;
            a = 1;

            if (item.GetComponent<Weapon>() == true)
                weapItem = (Weapon)_item;
        }
        else
            weapItem = null;

        SetColor(a);
    }

    //�̹����� ���İ� ����
    public void SetColor(float value)
    {
        Color color = img_item.color;
        color.a = value;
        img_item.color = color;
    }

    //���� ������ �����
    public void DoDestroy()
    {
        ItemManager.Remove(item);
        item = null;
        weapItem = null;
        img_item.sprite = null;
        SetColor(0);
    }

    public void TakeItem()
    {
        item.Count--;
        if (item.Count == 0)
            DoDestroy();
        else
            SetSlot(item);
    }

    //��Ŭ���� �������� Ÿ�԰� �´� ���� ���Կ� �ٷ� �־���
    void ChangeItemData(Slot[] slot)
    {
        foreach (var item in slot)
        {
            if (item.slotInfo.item == null)
            {
                Item tmp = this.item;
                SetSlot(item.slotInfo.item);
                item.slotInfo.SetSlot(tmp);

                return;
            }
        }
        Debug.Log("���� ���Կ� �� ������ �����ϴ�.");
    }

    //RectTransform ��Ŀ, �Ǻ� ������ ����ֱ�
    void SetRectPos(int anchor, int pivot)
    {
        itemInfoUI.transform.SetParent(transform);
        
        rect.anchorMin = new Vector2(anchor, 1);
        rect.anchorMax = new Vector2(anchor, 1);
        rect.pivot = new Vector2(pivot, 1);
        rect.anchoredPosition = Vector2.zero;

        rect.SetParent(UIManager.Instance.transform);
        rect.SetAsLastSibling();
    }

    //���콺�� ���� ������ ���� �� 1ȸ ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null && !eventData.dragging)
        {
            itemInfoUI.gameObject.SetActive(true);
            inven.ShowItemInfo(item);

            //ĵ���� ���� ũ�� - ������ ����â UI ���� ũ��
            float standard = Screen.width - rect.sizeDelta.x;

            if (transform.position.x < standard)
                SetRectPos(1, 0);
            else
                SetRectPos(0, 1);
        }
    }

    //���콺�� ���� ������ �������� �� 1ȸ ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.gameObject.SetActive(false);
    }

    //��ư �̺�Ʈ, ���ǿ� �´� ChangeItemData �Լ� ȣ��
    public void OnPointerDown(PointerEventData eventData)
    {
        //��Ŭ���� ������ ��
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //���� ������ �������� �ְ� �Ϲ� �κ��丮 �����̶�� �������� Ÿ�Կ� ���� �˸��� ���� �������� ������ ����
            if (item != null)
            {
                if (slotType == SlotType.INVEN)
                {
                    if (item.itemstat.type == ItemType.ACCESSORY)
                        ChangeItemData(inven.equipAccessory);
                    else if (item.itemstat.type != ItemType.ETC)
                        ChangeItemData(inven.equipWeapons);
                }
                else
                {
                    ChangeItemData(inven.items);
                }
            }
            UIManager.Instance.SetWeaponUI();
        }
    }

    //�巡�� ���۽� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        //��Ŭ���� ������ ��
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                dragSlot.slotinfo = this;
                dragSlot.SetItem(item);
                dragSlot.transform.position = eventData.position;
                SetColor(0);
            }
        }
    }

    //�巡�� �ϴ� ���� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            dragSlot.transform.position = eventData.position;
        }
    }

    //�巡�װ� ������ �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        //��Ŭ���� ���� ���
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                //��Ŭ���� �� ��ġ���� ����ĳ��Ʈ ���� �� ���� �±׸� ���� UI�� ������ slot �־���
                var ped = new PointerEventData(null);
                ped.position = eventData.position;
                List<RaycastResult> results = new List<RaycastResult>();
                gr.Raycast(ped, results);

                Slot slot = null;

                foreach (var result in results)
                {
                    if (result.gameObject.tag == "Slot")
                    {
                        slot = result.gameObject.GetComponent<Slot>();
                        continue;
                    }
                }
                //slot�� �� ������ ���� ��� ����
                if (slot != null)
                {
                    SlotType slotType = SlotType.INVEN;
                    CheckItemType(ref slotType);

                    if (slot.slotInfo.slotType == slotType || slot.slotInfo.slotType == SlotType.REINFORCE || slot.slotInfo.slotType == SlotType.INVEN)
                    {
                        Item sitem = slot.slotInfo.item;
                        if (sitem != null)
                        {
                            if(sitem.itemstat.type == ItemType.ETC && slotType != SlotType.INVEN)
                            {
                                dragSlot.item = sitem;
                                SetColor(1);
                            }
                            else
                            {
                                if (item.itemstat.id == sitem.itemstat.id)
                                {
                                    int sum = item.Count + sitem.Count;
                                    if (sum <= sitem.itemstat.maxStack)
                                    {
                                        sitem.Count = sum;
                                        DoDestroy();
                                    }
                                    else
                                    {
                                        sitem.Count = sitem.itemstat.maxStack;
                                        item.Count -= sitem.itemstat.maxStack - sitem.Count;
                                    }
                                    dragSlot.item = sitem;
                                }
                                else
                                    item = sitem;
                            }
                        }
                        else
                            item = null;

                        SetSlot(item);
                        slot.slotInfo.SetSlot(dragSlot.item);
                        dragSlot.DragItemOff();
                    }
                    else
                    {
                        dragSlot.DragItemOff();
                        SetColor(1);
                    }
                }
                else
                {
                    //�κ� ������ �������� ������ �ı����� UI�� ���� / �巡�� ���� ������ ����� / �ش� �������� ���� ���� �̹��� ���İ��� 0���� �ٲٱ�
                    inven.OnDestroyItemUI(true);
                    inven.DestroySlot = this;
                    SetColor(0);
                    dragSlot.DragItemOff();
                }
                UIManager.Instance.SetWeaponUI();
            }
        }
    }

    //������ Ÿ���� ���ϴ� ���
    void CheckItemType(ref SlotType _slotType)
    {
        switch(item.itemstat.type)
        {
            case ItemType.ACCESSORY:
                _slotType = SlotType.EQUIPACCESSORY;
                return;
            case ItemType.ETC:
                _slotType = SlotType.INVEN;
                return;
        }
        _slotType = SlotType.EQUIPWEAPON;
    }
}
