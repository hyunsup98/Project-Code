using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Slot> slots = new List<Slot>();


    [Header("아이템 정보창에 쓰일 변수")]
    public GameObject itemInfoUI;
    public Image itemImg;
    public TMP_Text itemName;
    public TMP_Text itemType;
    public TMP_Text itemLore;
    public TMP_Text itemCount;

    [Header("인벤토리가 켜지고 꺼질 때 관련 변수")]
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

    //반환값이 true면 템이 인벤에 잘 들어갔단 소리, false면 인벤이 꽉 차서 템이 들어가지 않았단 소리
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

    //인벤토리 슬롯에 item이 몇 개 있는지 반환하는 함수
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

    //강화 등의 소모 컨텐츠로 아이템을 소모해 인벤토리에서 개수만큼 삭제
    public void DiscountItem(Item item, int count)
    {
        //빼야할 아이템의 개수
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

    //같은 아이템이 이미 인벤토리에 있을 때 개수 추가
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

    //isEnter는 마우스가 슬롯에 들어갔는지 빠져나갔는지 확인하는 변수
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
        itemCount.text = $"보유 개수: {item.Count}개";

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

    //인벤토리의active를 false로 바꿔버리면 슬롯의 쿨타임이 꺼질 땐 돌기 않기 때문에 포지션을 변경해 꺼진 것처럼 보여주기 위한 함수
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
