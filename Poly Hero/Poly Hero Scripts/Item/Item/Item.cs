using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Item : MonoBehaviour
{
    //������ ����
    public ItemStats itemstats;

    protected bool isCoroutine = false;

    [Space]
    //�κ� ��� ui�� ǥ���� �̹���
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
