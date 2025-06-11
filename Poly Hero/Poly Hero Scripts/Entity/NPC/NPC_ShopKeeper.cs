using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ShopKeeper : NPC
{
    [SerializeField] List<Item> items = new List<Item>();
    [SerializeField] ShopUI shopUI;

    private void Start()
    {
        Init();
        shopUI.SetShopSlot(items);
    }

    public override void NPCEvent()
    {
        if(items.Count > 0)
        {
            shopUI.gameObject.SetActive(true);
        }
    }

    protected override void ExitCollider()
    {
        if(shopUI.gameObject.activeSelf)
            shopUI.gameObject.SetActive(false);
    }
}
