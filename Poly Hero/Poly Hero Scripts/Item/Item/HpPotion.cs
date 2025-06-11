using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : Item, IUseable
{
    [Header("포션 관련 변수")]
    //회복시킬 힐량
    [SerializeField] private float healAmount;
    //도트힐이 들어간다면 몇 회 들어갈지
    [SerializeField] private float healCount;
    //포션을 사용하고 난 후 재사용 대기시간
    [SerializeField] private float coolTime;
    //회복되는 이펙트
    [SerializeField] private Effect healEffect;

    //아이템 사용
    public void Use(Entity entity, Slot slot)
    {
        if(Count <= 1)
        {
            slot.ClearSlot();
        }

        StartCoroutine(HealPotion(entity, slot));
    }

    //entity 회복, slot 체크(아이템 카운트가 0이 되면 슬롯 비우기)
    private IEnumerator HealPotion(Entity entity, Slot slot)
    {
        isCoroutine = true;
        Count--;
        if (Count > 0)
            slot.CheckItemCount();

        if (entity.stat.hp >= entity.stat.maxhp)
            yield return null;

        //슬롯 쿨타임 돌아가게 하기
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
