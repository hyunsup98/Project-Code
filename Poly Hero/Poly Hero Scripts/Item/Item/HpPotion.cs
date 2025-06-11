using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : Item, IUseable
{
    [Header("���� ���� ����")]
    //ȸ����ų ����
    [SerializeField] private float healAmount;
    //��Ʈ���� ���ٸ� �� ȸ ����
    [SerializeField] private float healCount;
    //������ ����ϰ� �� �� ���� ���ð�
    [SerializeField] private float coolTime;
    //ȸ���Ǵ� ����Ʈ
    [SerializeField] private Effect healEffect;

    //������ ���
    public void Use(Entity entity, Slot slot)
    {
        if(Count <= 1)
        {
            slot.ClearSlot();
        }

        StartCoroutine(HealPotion(entity, slot));
    }

    //entity ȸ��, slot üũ(������ ī��Ʈ�� 0�� �Ǹ� ���� ����)
    private IEnumerator HealPotion(Entity entity, Slot slot)
    {
        isCoroutine = true;
        Count--;
        if (Count > 0)
            slot.CheckItemCount();

        if (entity.stat.hp >= entity.stat.maxhp)
            yield return null;

        //���� ��Ÿ�� ���ư��� �ϱ�
        slot.CoolTime = coolTime;

        for (int i = 0; i < healCount; i++)
        {
            float sum = entity.stat.hp + healAmount;

            if(sum >= entity.stat.maxhp)
            {
                entity.stat.hp = entity.stat.maxhp;
            }
            else
            {
                entity.stat.hp = sum;
            }

            Effect effect = EffectManager.Instance.Get(healEffect, transform);
            effect.transform.position = entity.transform.position;
            UIManager.Instance.SetHealthBar();

            yield return new WaitForSeconds(1f);
        }

        isCoroutine = false;
    }
}
