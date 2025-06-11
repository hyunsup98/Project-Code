using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine;
using Unity.VisualScripting;
using System.Linq;

public class Inventory : DelayBehaviour
{
    public Sword SWORD;
    public Short_Sword SHORT_SWORD;
    public Bow BOW;
    public Magic MAGIC;
    public Item Candy;

    //버릴 아이템이 들어있던 슬롯을 가져오기
    public SlotInfo DestroySlot;

    //파괴할지 묻는 UI 오브젝트
    public GameObject destroyItemUI;

    public Transform itemTrans;

    //주/보조무기 슬롯
    public Slot[] equipWeapons;
    //장식품 슬롯
    public Slot[] equipAccessory;
    //인벤토리 슬롯
    public Slot[] items;

    [Space]
    [Header("아이템 정보창에 쓰일 변수")]
    public GameObject itemInfoUI;
    public UnityEngine.UI.Image itemImg;
    public TMP_Text itemName;
    public TMP_Text itemLore;
    public TMP_Text itemCount;


    protected override void Updates()
    {
        if (itemTrans == null)
        {
            itemTrans = ItemManager.Instance.transform;
        }
        //Test
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Item i = ItemManager.Get(SWORD, itemTrans);
            SetInven(i);
            UIManager.Instance.SetWeaponUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Item i = ItemManager.Get(SHORT_SWORD, itemTrans);
            SetInven(i);
            UIManager.Instance.SetWeaponUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Item i = ItemManager.Get(BOW, itemTrans);
            SetInven(i);
            UIManager.Instance.SetWeaponUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Item i = ItemManager.Get(MAGIC, itemTrans);
            SetInven(i);
            UIManager.Instance.SetWeaponUI();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Item i = ItemManager.Get(Candy, itemTrans);
            SetInven(i);
            UIManager.Instance.SetWeaponUI();
        }
    }

    //인벤토리가 켜질 때 플레이어 스킬 작동x
    private void OnEnable()
    {
        GameManager.Instance.state = GameState.Pause;
    }

    //인벤토리가 꺼질 때 플레이어 스킬 작동o
    private void OnDisable()
    {
        GameManager.Instance.state = GameState.Play;

        if (itemInfoUI.activeSelf == true)
            itemInfoUI.SetActive(false);
    }

    //실질적으로 인벤토리 슬롯에 템을 넣어주는 함수
    public void SetInven(Item _item)
    {
        Slot slot = AddInven(_item);

        if (slot != null)
            slot.slotInfo.SetSlot(_item);
    }

    //아이템 획득 시 빈 슬롯의 위치를 반환
    Slot AddInven(Item _item)
    {
        //장비나 장신구를 먹을 시 착용슬롯을 우선 체크한다
        Slot slot = null;

        if (_item.itemstat.type == ItemType.ACCESSORY)
        {
            slot = CheckEmptySlot(ref equipAccessory, ref _item);
        }
        else if (_item.itemstat.type != ItemType.ETC)
        {
            slot = CheckEmptySlot(ref equipWeapons, ref _item);
        }

        //착용슬롯이 다 차있을 경우 slot = null 따라서 null일 경우 인벤토리 슬롯 체크
        if (slot != null)
            return slot;
        else
        {
            slot = CheckEmptySlot(ref items, ref _item);

            if(slot != null)
                return slot;
        }
        return null;
    }

    //인벤토리 슬롯중 빈 슬롯이 있으면 해당 슬롯을 반환, 없으면 null 반환
    Slot CheckEmptySlot(ref Slot[] _slot, ref Item _item)
    {
        foreach(var s in _slot)
        {
            if(s.slotInfo.item != null && _item.itemstat.type == ItemType.ETC)
            {
                Item sitem = s.slotInfo.item;
                if (sitem.itemstat.id == _item.itemstat.id)
                {
                    int sum = sitem.Count + _item.Count;
                    if (sum <= sitem.itemstat.maxStack)
                    {
                        sitem.Count = sum;
                        ItemManager.Remove(_item);

                        return null;
                    }
                    else
                    {
                        sitem.Count = sitem.itemstat.maxStack;
                        _item.Count -= _item.itemstat.maxStack - sitem.Count;
                        Debug.Log(_item.Count);
                    }
                }
            }
        }
        foreach (var slot in _slot)
        {
            if (slot.slotInfo.item == null)
                return slot;
        }
        return null;
    }

    public SlotInfo CheckItem(int id)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].slotInfo.item != null)
            {
                if (items[i].slotInfo.item.itemstat.id == id)
                {
                    return items[i].slotInfo;
                }
            }
        }
        return null;
    }

    //인벤토리 끄기
    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    //인벤토리 초기화
    public void Clear()
    {
        foreach(var slot in items)
        {
            slot.slotInfo.DoDestroy();
        }
        foreach (var slot in equipAccessory)
        {
            slot.slotInfo.DoDestroy();
        }
        foreach (var slot in equipWeapons)
        {
            slot.slotInfo.DoDestroy();
        }
    }
    //아이템을 인벤 밖 화면에 드래그할 경우 파괴 UI 실행
    public void OnDestroyItemUI(bool isOn)
    {
        destroyItemUI.SetActive(isOn);
    }

    //파괴 여부에서 예를 선택한 경우 파괴
    public void OnDestroyOk()
    {
        DestroySlot.DoDestroy();
        BtnDestroyFunc();
        UIManager.Instance.SetWeaponUI();
    }

    //파괴 여부에서 아니오를 선택한 경우 슬롯에 다시 이미지 보이기
    public void OnDestroyCancel()
    {
        DestroySlot.SetColor(1);
        BtnDestroyFunc();
    }

    //아이템 파괴할 때 공통적으로 실행할 기능들
    void BtnDestroyFunc()
    {
        OnDestroyItemUI(false);
    }

    //아이템 정보창에 실질적인 정보를 세팅하는 함수
    public void ShowItemInfo(Item item)
    {
        itemImg.sprite = item.render.sprite;
        itemName.text = item.itemstat.name;

        ItemGrade grade = item.itemstat.grade;
        if (grade == ItemGrade.COMMON)
            itemName.color = Color.white;
        else if (grade == ItemGrade.NORMAL)
            itemName.color = Color.green;
        else if (grade == ItemGrade.RARE)
            itemName.color = Color.blue;
        else if (grade == ItemGrade.EPIC)
            itemName.color = new Color32(153, 50, 204, 255);
        else
            itemName.color = new Color32(255, 215, 0, 255);

        itemLore.text = item.itemstat.lore;
        if (item.Count > 1)
            itemCount.text = item.Count.ToString();
        else
            itemCount.text = string.Empty;
    }
}
