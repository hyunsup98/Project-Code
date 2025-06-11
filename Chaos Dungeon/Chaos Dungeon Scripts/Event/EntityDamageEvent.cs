using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//대상이 다른 대상에게 피해를 입힐때 발생
//공격한놈 , 피격대상 , 피해

//공격타입 일반 , 지속 , 고정으로 우선 분류해둠
public enum AttackType
{
    NORMAL,
    DOT,        //도트 & 출혈
    FIXED,
    COR         //부식(방깎)
}

public class EntityDamageEvent
{
    bool isCancel = false;

    Entity damager;
    Entity target;

    float damage;
    float first_damage;

    public AttackType type;
    
    public EntityDamageEvent(Entity damager, Entity target, float damage, AttackType type)
    {
        this.damager = damager;
        this.target = target;
        first_damage = this.damage = damage;
        this.type = type;
        damager.AttackEvent(this);
        target.HitEvent(this);
    }

    public bool Cancel
    {
        get { return isCancel; }
        set { isCancel = value; }
    }

    public float Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public float GetFristDamage()
    {
        return first_damage;
    }

    public Entity GetTarget()
    {
        return target;
    }

    public Entity GetDamager()
    {
        return damager;
    }

    public AttackType GetAttackType()
    {
        return type;
    }
}
