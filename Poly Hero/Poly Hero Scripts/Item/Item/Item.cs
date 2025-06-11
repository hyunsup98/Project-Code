using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Item : MonoBehaviour
{
    //아이템 스탯
    public ItemStats itemstats;

    protected bool isCoroutine = false;

    [Space]
    //인벤 등등 ui에 표시할 이미지
    public Sprite itemSprite;

    protected int count = 1;
    public int Count
    {
        get { return count; }
        set
        {
            if(value > itemstats.maxStack)
                value = itemstats.maxStack;
            else if(value <= 0)
            {
                StartCoroutine(takeItem());
                return;
            }
            count = value;
        }
    }

    protected IEnumerator takeItem()
    {
        yield return new WaitUntil(() => !isCoroutine);

        ItemManager.Instance.Take(this);
    }

    private void OnEnable()
    {
        Count = 1;
    }
}
