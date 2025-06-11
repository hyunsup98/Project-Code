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

    //아이템 정보를 보여주는 UI창
    public GameObject itemInfoUI;
    RectTransform rect;

    public Item item;
    //슬롯에 넣은 아이템이 Weapon이면 weapItem에 저장
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

    //인벤토리에 아이템 이미지 표시, a는 알파값
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

    //이미지의 알파값 변경
    public void SetColor(float value)
    {
        Color color = img_item.color;
        color.a = value;
        img_item.color = color;
    }

    //슬롯 데이터 지우기
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

    //우클릭시 아이템의 타입과 맞는 착용 슬롯에 바로 넣어줌
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
        Debug.Log("장착 슬롯에 빈 공간이 없습니다.");
    }

    //RectTransform 앵커, 피봇 포지션 잡아주기
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

    //마우스가 슬롯 영역을 들어올 때 1회 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null && !eventData.dragging)
        {
            itemInfoUI.gameObject.SetActive(true);
            inven.ShowItemInfo(item);

            //캔버스 가로 크기 - 아이템 정보창 UI 가로 크기
            float standard = Screen.width - rect.sizeDelta.x;

            if (transform.position.x < standard)
                SetRectPos(1, 0);
            else
                SetRectPos(0, 1);
        }
    }

    //마우스가 슬롯 영역을 빠져나갈 때 1회 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.gameObject.SetActive(false);
    }

    //버튼 이벤트, 조건에 맞는 ChangeItemData 함수 호출
    public void OnPointerDown(PointerEventData eventData)
    {
        //우클릭을 눌렀을 때
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //현재 슬롯의 아이템이 있고 일반 인벤토리 슬롯이라면 아이템의 타입에 따라 알맞은 착용 슬롯으로 데이터 전달
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

    //드래그 시작시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        //좌클릭을 눌렀을 때
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

    //드래그 하는 도중 호출
    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            dragSlot.transform.position = eventData.position;
        }
    }

    //드래그가 끝났을 때 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        //좌클릭을 뗐을 경우
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                //좌클릭을 뗀 위치에서 레이캐스트 실행 후 슬롯 태그를 가진 UI가 있으면 slot 넣어줌
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
                //slot에 들어간 슬롯이 있을 경우 실행
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
                    //인벤 밖으로 아이템을 버릴시 파괴여부 UI를 띄우기 / 드래그 슬롯 정보를 지우기 / 해당 아이템의 기존 슬롯 이미지 알파값을 0으로 바꾸기
                    inven.OnDestroyItemUI(true);
                    inven.DestroySlot = this;
                    SetColor(0);
                    dragSlot.DragItemOff();
                }
                UIManager.Instance.SetWeaponUI();
            }
        }
    }

    //슬롯의 타입을 구하는 기능
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
