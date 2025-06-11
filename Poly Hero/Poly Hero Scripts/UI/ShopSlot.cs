using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    public Image itemImg;
    public TMP_Text itemName;
    public TMP_Text gold;

    Item item;

    public void SetShopSlotData(Item item)
    {
        this.item = item;
        itemImg.sprite = item.itemSprite;
        itemName.text = item.itemstats.name;
        gold.text = item.itemstats.buyPrice.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            UIManager.Instance.itemBuyUI.SetBuyItem(item);
        }
    }
}
