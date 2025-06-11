using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemDropTable : ScriptableObject
{
    //등급 랜덤 드랍일 경우 드랍할 템 개수
    public int randomMinCount, randomMaxCount;

    public bool isCommon;
    public bool isNormal;
    public bool isRare;
    public bool isEpic;
    public bool isLegend;

    [System.Serializable]
    public class Items
    {
        public Item item;
        public int minCount;
        public int maxCount;
        public float dropPercent;
    }

    public List<Items> items = new List<Items>();


    List<Item> AllGradeItem()
    {
        if (randomMaxCount <= 0)
            return null;

        List<Item> listItem = new List<Item>();

        if (isCommon)
            listItem.AddRange(CheckGrade(ItemGrade.COMMON));
        if (isNormal)
            listItem.AddRange(CheckGrade(ItemGrade.NORMAL));
        if (isRare)
            listItem.AddRange(CheckGrade(ItemGrade.RARE));
        if (isEpic)
            listItem.AddRange(CheckGrade(ItemGrade.EPIC));
        if (isLegend)
            listItem.AddRange(CheckGrade(ItemGrade.LEGEND));

        List<Item> result = new List<Item>();

        int randomCount = Random.Range(randomMinCount, randomMaxCount + 1);

        for (int i = 0; i < randomCount; i++)
        {
            int randNum = Random.Range(0, listItem.Count);
            result.Add(listItem[randNum]);
        }

        return result;
    }

    List<Item> CheckGrade(ItemGrade _grade)
    {
        List<Item> listItem = new List<Item>();

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item.itemstat.grade == _grade)
                listItem.Add(items[i].item);
        }
        return listItem;
    }

    List<Item> PickItems()
    {
        List<Item> l = new List<Item>();

        if (items.Count <= 0)
            return null;

        for (int i = 0; i < items.Count; i++)
        {
            int per = Random.Range(1, 101);
            if (per <= items[i].dropPercent)
            {
                int randCount = Random.Range(items[i].minCount, items[i].maxCount + 1);
                for (int j = 0; j < randCount; j++)
                {
                    l.Add(items[i].item);
                }
            }
        }
        return l;
    }

    public void DropItems(Transform trans)
    {
        List<Item> l = AllGradeItem();

        l ??= PickItems();

        if (l.Count <= 0 || l == null)
            return;

        for (int i = 0; i < l.Count; i++)
        {
            Item it = ItemManager.Get(l[i], trans);

            it.transform.localScale = new Vector3(1, 1, 1);
            it.BoolChange();
            float x = Random.Range(-0.3f, 0.3f);
            float power = Random.Range(1, 2.0f);

            it.GetComponent<Rigidbody2D>().AddForce(new Vector2(x, 1) * power, ForceMode2D.Impulse);
        }
    }
}
