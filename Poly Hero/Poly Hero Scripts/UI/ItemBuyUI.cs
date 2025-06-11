using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemBuyUI : MonoBehaviour
{
    [SerializeField] TMP_InputField inputcount;     //추가될 아이템 개수
    Item item;      //구매 후 인벤에 추가될 아이템

    int count = 0;
    public void SetBuyItem(Item item)
    {
        this.item = item;
        gameObject.SetActive(true);
    }

    //구매 확인 버튼 클릭시
    public void OnBuyBtn()
    {
        if (item == null || count == 0) return;

        if (count > item.itemstats.maxStack)
            count = item.itemstats.maxStack;

        int totalPrice = item.itemstats.buyPrice * count;

        //플레이어가 아이템을 구매할 골드를 소지 시
        if (GameManager.Instance.player.Money >= totalPrice)
        {
            Item copyItem = ItemManager.Instance.Get(item, transform);
            copyItem.Count = count;
            UIManager.Instance.inventory.AddItem(copyItem);
            GameManager.Instance.player.Money -= totalPrice;
            gameObject.SetActive(false);
        }
        else  //골드 소지 x
        {

        }
    }

    //구매 취소 버튼 클릭시
    public void OnCancleBtn()
    {
        gameObject.SetActive(false);
    }

    //구매할 아이템 개수 입력 후 다른 공간을 클릭할 경우 실행할 함수
    public void OnValueChangeCount()
    {
        string text = inputcount.text;

        if (int.TryParse(text, out count))
        {
            count = int.Parse(text);
        }

        if (count > item.itemstats.maxStack)
        {
            inputcount.text = item.itemstats.maxStack.ToString();
        }
    }

    private void OnEnable()
    {
        transform.SetAsLastSibling();
    }

    private void OnDisable()
    {
        inputcount.text = string.Empty;
        item = null;
        count = 0;
    }
}
