using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] ShopSlot shopSlot;
    [SerializeField] Transform slotTrans;   //shopSlot을 생성할 트랜스폼

    List<ShopSlot> shopSlots = new List<ShopSlot>();

    public void SetShopSlot(List<Item> item)
    {
        foreach(var i in item)
        {
            ShopSlot slot = Instantiate(shopSlot, slotTrans);
            slot.SetShopSlotData(i);
            shopSlots.Add(slot);
        }
    }

    private void OnDisable()
    {
        if(shopSlots.Count > 0)
        {
            shopSlots.Clear();
        }

        if (UIManager.Instance.dialogUI.gameObject.activeSelf)
        {
            UIManager.Instance.dialogUI.gameObject.SetActive(false);
        }
    }
}
