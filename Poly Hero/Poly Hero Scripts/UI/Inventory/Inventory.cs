using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Slot> slots = new List<Slot>();


    [Header("������ ����â�� ���� ����")]
    public GameObject itemInfoUI;
    public Image itemImg;
    public TMP_Text itemName;
    public TMP_Text itemType;
    public TMP_Text itemLore;
    public TMP_Text itemCount;

    [Header("�κ��丮�� ������ ���� �� ���� ����")]
    public bool isOn = false;
    [SerializeField] private Vector3 onPos;
    private Vector3 offPos;
    [SerializeField] private RectTransform rect;

    private void Start()
    {
        Vector3 pos = onPos;
        pos.y += 2000;
        offPos = pos;
    }

    //��ȯ���� true�� ���� �κ��� �� ���� �Ҹ�, false�� �κ��� �� ���� ���� ���� �ʾҴ� �Ҹ�
    public bool AddItem(Item item)
    {
        Item newItem = AddCountItem(item);

        if(newItem != null)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item == null)
                {
                    Item invenITem = ItemManager.Instance.Get(newItem, transform);
                    invenITem.Count = newItem.Count;
                    slots[i].AddSlotData(invenITem);
                    return true;
                }
            }
            ItemManager.Instance.Take(item);
            return false;
        }
        return true;
    }

    public void MoveItem(Item item)
    {
        Item newItem = AddCountItem(item);

        if (newItem != null)
        {
            {
                for(int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].item == null)
                    {
                        slots[i].AddSlotData(newItem);
                        return;
                    }
                }
                ItemManager.Instance.Take(item);
            }
        }
    }

    //�κ��丮 ���Կ� item�� �� �� �ִ��� ��ȯ�ϴ� �Լ�
    public int ItemCount(Item item)
    {
        int sum = 0;

        foreach(var slot in slots)
        {
            if(slot.item != null)
            {
                sum += slot.item.itemstats.id == item.itemstats.id ? slot.item.Count : 0;
            }
        }

        return sum;
    }

    //��ȭ ���� �Ҹ� �������� �������� �Ҹ��� �κ��丮���� ������ŭ ����
    public void DiscountItem(Item item, int count)
    {
        //������ �������� ����
        int sub = count;

        foreach(var slot in slots)
        {
            if(slot.item != null)
            {
                if (slot.item.itemstats.id == item.itemstats.id)
                {
                    if (slot.item.Count > sub)
                    {
                        slot.item.Count -= sub;
                        slot.CheckItemCount();
                        return;
                    }
                    else
                    {
                        sub -= slot.item.Count;
                        slot.item.Count = 0;
                    }
                    slot.CheckItemCount();
                }
            }
        }
    }

    //���� �������� �̹� �κ��丮�� ���� �� ���� �߰�
    Item AddCountItem(Item item)
    {
        Item newItem = item;
        for(int i = 0; i < slots.Count; i++)
        {
            Item slotItem = slots[i].item;

            if(slotItem != null)
            {
                if(slotItem.itemstats.id == newItem.itemstats.id)
                {
                    if(slotItem.Count < slotItem.itemstats.maxStack)
                    {
                        int sum = slotItem.Count + newItem.Count;

                        if(sum <= slotItem.itemstats.maxStack)
                        {
                            slotItem.Count = sum;
                            slots[i].SetCountText();
                            return null;
                        }
                        else
                        {
                            slotItem.Count = slotItem.itemstats.maxStack;
                            newItem.Count = sum - slotItem.itemstats.maxStack;
                            slots[i].SetCountText();
                        }
                    }
                }
            }
        }
        return newItem;
    }

    //isEnter�� ���콺�� ���Կ� ������ ������������ Ȯ���ϴ� ����
    public void ShowItemInfo(Item item, bool isEnter)
    {
        itemInfoUI.SetActive(isEnter);

        if (!isEnter)
            return;

        itemImg.sprite = item.itemSprite;
        itemName.text = item.itemstats.name;
        if(item.GetComponent<Equip>() != null)
        {
            itemName.text += $" +{item.GetComponent<Equip>().stats.level}";
        }
        itemType.text = item.itemstats.type;
        itemLore.text = item.itemstats.description;
        itemCount.text = $"���� ����: {item.Count}��";

        ItemGrade grade = item.itemstats.grade;
        switch(grade)
        {
            case ItemGrade.Common:
                itemType.color = Color.white;
                break;
            case ItemGrade.Rare:
                itemType.color = Color.blue;
                break;
            case ItemGrade.Epic:
                itemType.color = new Color32(153, 50, 204, 255);
                break;
        }
    }

    //�κ��丮��active�� false�� �ٲ������ ������ ��Ÿ���� ���� �� ���� �ʱ� ������ �������� ������ ���� ��ó�� �����ֱ� ���� �Լ�
    public void OnInventory()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        GameManager.Instance.SetGameState(gameObject, true);
        
        rect.localPosition = onPos;
        isOn = true;
    }

    public void OffInventory()
    {
        onPos = rect.localPosition;
        GameManager.Instance.SetGameState(gameObject, false);
        rect.localPosition = offPos;
        isOn = false;
        if (itemInfoUI.activeSelf)
        {
            itemInfoUI.SetActive(false);
        }
    }
}
