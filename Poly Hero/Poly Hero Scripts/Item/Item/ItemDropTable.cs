using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDropTable : ScriptableObject
{
    [SerializeField] private RandomItem randomITem;     //�������� ���� ���������� �������� ���� �־���
    [SerializeField] private RandomItem common;   //Ŀ��
    [SerializeField] private RandomItem rare;     //����
    [SerializeField] private RandomItem epic;     //����

    public int money = 0;

    [System.Serializable]
    public class Items
    {
        public Item item;
        public int count;
        public int minCount;
        public int maxCount;
        public float dropPercent;
    }

    public List<Items> items = new List<Items>();

    List<Items> PickItems()
    {
        List<Items> itemList = new List<Items>();

        if (items.Count <= 0)
            return null;

        for (int i = 0; i < items.Count; i++)
        {
            int per = Random.Range(1, 101);
            if (per <= items[i].dropPercent)
            {
                int randCount = Random.Range(items[i].minCount, items[i].maxCount + 1);

                items[i].count = randCount;
                itemList.Add(items[i]);
            }
        }
        return itemList;
    }

    //trans�� ������ ���
    public void DropItems(Transform trans)
    {
        List<Items> itemList = PickItems();

        if (itemList.Count <= 0 || itemList == null)
            return;

        if (money > 0)
            GameManager.Instance.player.Money += money;

        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = itemList[i].item;
            item.Count = itemList[i].count;
            //�������� �������� ���� ������ �����տ� �־���
            RandomItem ranItem = RandomItemManager.Instance.Get(randomITem != null ? randomITem : CheckItemGrade(item), trans);

            Vector3 pos = trans.position;
            pos.y += 1f;
            ranItem.transform.position = pos;
            ranItem.item = item;

            float x = Random.Range(0, 1.0f);
            float y = Random.Range(0.2f, 1f);
            float z = Random.Range(0, 1.0f);

            ranItem.rigid.AddForce(new Vector3(x, y, z), ForceMode.Impulse);
        }
    }

    //������ ����� üũ�� �ش� ��޿� �´� ��������� �������� ����
    RandomItem CheckItemGrade(Item item)
    {
        switch (item.itemstats.grade)
        {
            case ItemGrade.Common:
                return common;
            case ItemGrade.Rare:
                return rare;
            case ItemGrade.Epic:
                return epic;
        }

        return null;
    }

    //�κ��丮�� �ٷ� �� �߰�
    public void AddItems(Transform trans)
    {
        List<Items> itemList = PickItems();

        if (itemList.Count <= 0 || itemList == null)
            return;

        Queue<Item> iQueue = new Queue<Item>();

        if (money > 0)
            GameManager.Instance.player.Money += money;

        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = itemList[i].item;
            item.Count = itemList[i].count;

            bool isAdd = UIManager.Instance.inventory.AddItem(item);

            if(!isAdd)
            {
                iQueue.Enqueue(item);
            }
        }

        for(int i = 0; i < iQueue.Count; i++)
        {
            Item item = iQueue.Dequeue();
            RandomItem ranItem = RandomItemManager.Instance.Get(randomITem != null ? randomITem : CheckItemGrade(item), trans);
            ranItem.item = item;

            float x = Random.Range(0, 1.0f);
            float y = Random.Range(1.0f, 2.0f);
            float z = Random.Range(0, 1.0f);

            ranItem.rigid.AddForce(new Vector3(x, y, z), ForceMode.Impulse);
        }
    }
}
