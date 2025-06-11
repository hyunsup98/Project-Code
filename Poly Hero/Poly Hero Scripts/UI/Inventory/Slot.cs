using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public enum SlotType
{
    Slot,
    QuickSlot,
    ForceSlot,
    SkillSlot
}

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;
    [SerializeField] private SlotType slotType;
    [SerializeField] private Image itemImg;

    GraphicRaycaster gr;

    //��Ÿ���� ǥ�õ� �̹���
    [SerializeField] Image coolImg;
    private float coolTime = 0;
    public float CoolTime
    {
        get { return coolTime; }

        set
        {
            coolTime = value;

            if (value > 0)
            {
                StartCoroutine(SetSlotCool(value));
            }
        }
    }

    //���� ī��Ʈ ǥ��
    [SerializeField] TMP_Text countText;

    Transform transItemInfoUI;
    RectTransform rect;

    void Start()
    {
        transItemInfoUI = UIManager.Instance.inventory.itemInfoUI.transform;
        rect = transItemInfoUI.GetComponent<RectTransform>();
        gr = UIManager.Instance.gr;

        UIManager.Instance.InitSlots += ClearSlot;
    }

    //���� ������ ���콺�� ������ �� 1ȸ ����
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.SetSoundString("inventorySound", transform);

        if (item != null && !eventData.dragging && slotType != SlotType.QuickSlot)
        {
            UIManager.Instance.inventory.ShowItemInfo(item, true);

            //ĵ���� ���� ũ�� - ������ ����â ui ���� ũ�� (+50�� ������ ���� ũ���� ������ ������ ��)
            float widthStandard = Screen.width - (rect.sizeDelta.x + 50);
            float heightStandard = Screen.height - rect.sizeDelta.y;

            float anchorX, anchorY, pivotX, pivotY;

            if (transform.position.x < widthStandard)
            {
                anchorX = 1;
                pivotX = 0;
            }
            else
            {
                anchorX = 0;
                pivotX = 1;
            }

            if (transform.position.y < heightStandard)
            {
                anchorY = 0;
                pivotY = 0;
            }
            else
            {
                anchorY = 1;
                pivotY = 1;
            }

            SetItemInfoRectPos(anchorX, anchorY, pivotX, pivotY);
        }
    }

    //���� �������� ���콺�� �������� �� 1ȸ ����
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.inventory.ShowItemInfo(item, false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (slotType == SlotType.Slot)
            {
                if (item != null && item.itemstats.itemType == ItemType.Use && coolTime <= 0)
                {
                    item.GetComponent<IUseable>()?.Use(GameManager.Instance.player, this);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                DragSlot.Instance.dragslot = this;
                DragSlot.Instance.SetItemData(item.itemSprite);
                DragSlot.Instance.transform.position = eventData.position;

                if (countText != null)
                {
                    countText.text = string.Empty;
                }
                SetColor(0);

                if (slotType == SlotType.ForceSlot)
                {
                    UIManager.Instance.reinForceUI.item = null;
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                SetColor(1);
                CheckItemCount();

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
                if (slot == null && slotType == SlotType.Slot)
                {
                    UIManager.Instance.destroyUI.SetDestroyItem(this);
                }
            }
        }
        DragSlot.Instance.Setcolor(0);
        DragSlot.Instance.dragslot = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.Instance.dragslot != null)
        {
            if (DragSlot.Instance.dragslot != this)
            {
                Item dragItem = DragSlot.Instance.dragslot.item;
                if (slotType == SlotType.Slot)
                {
                    ChangeSlotData();
                }
                else if (slotType == SlotType.QuickSlot)
                {
                    if (dragItem.itemstats.itemType == ItemType.HandEquip || dragItem.itemstats.itemType == ItemType.Use)
                    {
                        ChangeSlotData();
                    }
                }
                if (GameManager.Instance.player.playerWeapon.gameObject == dragItem.gameObject)
                {
                    GameManager.Instance.player.RemoveTool();
                }
            }

            if (slotType == SlotType.ForceSlot)
            {
                if (DragSlot.Instance.dragslot.item.GetComponent<IForce>() != null)
                {
                    ChangeSlotData();

                    if (item != null)
                    {
                        UIManager.Instance.reinForceUI.ShowIngredient(item.GetComponent<Equip>());
                    }
                }
            }
        }
    }

    public void ChangeSlotData()
    {
        Item dragItem = DragSlot.Instance.dragslot.item;

        if (item != null)
        {
            if (item.itemstats.id != dragItem.itemstats.id)
            {
                DragSlot.Instance.dragslot.AddSlotData(item);
            }
            else
            {
                int sum = item.Count + dragItem.Count;

                if (sum <= item.itemstats.maxStack)
                {
                    item.Count = sum;
                    DragSlot.Instance.dragslot.ClearSlot();
                }
                else
                {
                    item.Count = dragItem.itemstats.maxStack;
                    dragItem.Count = sum - item.itemstats.maxStack;

                    DragSlot.Instance.dragslot.AddSlotData(dragItem);
                }

                SetCountText();
                return;
            }
        }
        else
            DragSlot.Instance.dragslot.ClearSlot();

        AddSlotData(dragItem);
    }

    //���� ������ �߰��ϱ�
    public void AddSlotData(Item item)
    {
        this.item = item;
        itemImg.sprite = item.itemSprite;

        SetCountText();
        SetColor(1);
    }

    public void SetCountText()
    {
        if (item != null && countText != null)
        {
            if (item.Count >= 1 && item.itemstats.maxStack > 1)
            {
                countText.text = item.Count.ToString();
            }
            else
            {
                countText.text = string.Empty;
            }
        }
    }

    //���� ������ ����
    public void ClearSlot()
    {
        item = null;
        itemImg.sprite = null;
        if (countText != null)
            countText.text = string.Empty;
        SetColor(0);
    }

    //������ ī��Ʈ üũ�ϱ�
    public void CheckItemCount()
    {
        if (item.Count <= 0 || item == null)
        {
            ItemManager.Instance.Take(item);
            ClearSlot();
        }
        else
        {
            SetCountText();
        }
    }

    //���� �̹��� ���İ� ����
    public void SetColor(float alpha)
    {
        Color color = itemImg.color;
        color.a = alpha;
        itemImg.color = color;
    }

    //���� ��Ÿ�� �����ϱ�
    public IEnumerator SetSlotCool(float _coolTime)
    {
        coolTime = _coolTime;

        while (coolTime >= 0)
        {
            coolImg.fillAmount = coolTime / _coolTime;
            coolTime -= Time.deltaTime;

            yield return null;
        }
    }

    //������ ����â ��Ŀ, �Ǻ� ������ ����ֱ�
    void SetItemInfoRectPos(float anchorX, float anchorY, float pivotX, float pivotY)
    {
        transItemInfoUI.SetParent(transform);

        rect.anchorMin = new Vector2(anchorX, anchorY);
        rect.anchorMax = new Vector2(anchorX, anchorY);
        rect.pivot = new Vector2(pivotX, pivotY);
        rect.anchoredPosition = Vector2.zero;

        rect.SetParent(UIManager.Instance.transform);
        rect.SetAsLastSibling();
    }
}
