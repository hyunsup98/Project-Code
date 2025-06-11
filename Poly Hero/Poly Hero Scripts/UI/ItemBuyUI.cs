using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemBuyUI : MonoBehaviour
{
    [SerializeField] TMP_InputField inputcount;     //�߰��� ������ ����
    Item item;      //���� �� �κ��� �߰��� ������

    int count = 0;
    public void SetBuyItem(Item item)
    {
        this.item = item;
        gameObject.SetActive(true);
    }

    //���� Ȯ�� ��ư Ŭ����
    public void OnBuyBtn()
    {
        if (item == null || count == 0) return;

        if (count > item.itemstats.maxStack)
            count = item.itemstats.maxStack;

        int totalPrice = item.itemstats.buyPrice * count;

        //�÷��̾ �������� ������ ��带 ���� ��
        if (GameManager.Instance.player.Money >= totalPrice)
        {
            Item copyItem = ItemManager.Instance.Get(item, transform);
            copyItem.Count = count;
            UIManager.Instance.inventory.AddItem(copyItem);
            GameManager.Instance.player.Money -= totalPrice;
            gameObject.SetActive(false);
        }
        else  //��� ���� x
        {

        }
    }

    //���� ��� ��ư Ŭ����
    public void OnCancleBtn()
    {
        gameObject.SetActive(false);
    }

    //������ ������ ���� �Է� �� �ٸ� ������ Ŭ���� ��� ������ �Լ�
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
