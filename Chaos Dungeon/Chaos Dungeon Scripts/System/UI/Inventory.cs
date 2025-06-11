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

    //���� �������� ����ִ� ������ ��������
    public SlotInfo DestroySlot;

    //�ı����� ���� UI ������Ʈ
    public GameObject destroyItemUI;

    public Transform itemTrans;

    //��/�������� ����
    public Slot[] equipWeapons;
    //���ǰ ����
    public Slot[] equipAccessory;
    //�κ��丮 ����
    public Slot[] items;

    [Space]
    [Header("������ ����â�� ���� ����")]
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

    //�κ��丮�� ���� �� �÷��̾� ��ų �۵�x
    private void OnEnable()
    {
        GameManager.Instance.state = GameState.Pause;
    }

    //�κ��丮�� ���� �� �÷��̾� ��ų �۵�o
    private void OnDisable()
    {
        GameManager.Instance.state = GameState.Play;

        if (itemInfoUI.activeSelf == true)
            itemInfoUI.SetActive(false);
    }

    //���������� �κ��丮 ���Կ� ���� �־��ִ� �Լ�
    public void SetInven(Item _item)
    {
        Slot slot = AddInven(_item);

        if (slot != null)
            slot.slotInfo.SetSlot(_item);
    }

    //������ ȹ�� �� �� ������ ��ġ�� ��ȯ
    Slot AddInven(Item _item)
    {
        //��� ��ű��� ���� �� ���뽽���� �켱 üũ�Ѵ�
        Slot slot = null;

        if (_item.itemstat.type == ItemType.ACCESSORY)
        {
            slot = CheckEmptySlot(ref equipAccessory, ref _item);
        }
        else if (_item.itemstat.type != ItemType.ETC)
        {
            slot = CheckEmptySlot(ref equipWeapons, ref _item);
        }

        //���뽽���� �� ������ ��� slot = null ���� null�� ��� �κ��丮 ���� üũ
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

    //�κ��丮 ������ �� ������ ������ �ش� ������ ��ȯ, ������ null ��ȯ
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

    //�κ��丮 ����
    public void OnClickClose()
    {
        gameObject.SetActive(false);
    }

    //�κ��丮 �ʱ�ȭ
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
    //�������� �κ� �� ȭ�鿡 �巡���� ��� �ı� UI ����
    public void OnDestroyItemUI(bool isOn)
    {
        destroyItemUI.SetActive(isOn);
    }

    //�ı� ���ο��� ���� ������ ��� �ı�
    public void OnDestroyOk()
    {
        DestroySlot.DoDestroy();
        BtnDestroyFunc();
        UIManager.Instance.SetWeaponUI();
    }

    //�ı� ���ο��� �ƴϿ��� ������ ��� ���Կ� �ٽ� �̹��� ���̱�
    public void OnDestroyCancel()
    {
        DestroySlot.SetColor(1);
        BtnDestroyFunc();
    }

    //������ �ı��� �� ���������� ������ ��ɵ�
    void BtnDestroyFunc()
    {
        OnDestroyItemUI(false);
    }

    //������ ����â�� �������� ������ �����ϴ� �Լ�
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
