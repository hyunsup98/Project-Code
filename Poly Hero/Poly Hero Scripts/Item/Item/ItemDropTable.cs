using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDropTable : ScriptableObject
{
    [SerializeField] private RandomItem randomITem;     //고정으로 나올 랜덤아이템 프리팹은 여기 넣어줌
    [SerializeField] private RandomItem common;   //커먼
    [SerializeField] private RandomItem rare;     //레어
    [SerializeField] private RandomItem epic;     //에픽

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

    //trans에 아이템 드랍
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
            //실질적인 아이템을 랜덤 아이템 프리팹에 넣어줌
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

    //아이템 등급을 체크해 해당 등급에 맞는 드랍아이템 프리팹을 생성
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

    //인벤토리에 바로 템 추가
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
