using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkEvent
{
    public Entity attacker;           //공격 시전 대상
    public Entity damager;            //공격 당한 대상
    public float damage;              //대미지
    public float originDamage;        //연산 되지않은 순수 대미지
    public DamageType damageType;     //대미지 타입

    public bool isCancle = false;

    public AtkEvent(Entity attacker, Entity damager, float damage, DamageType type)
    {
        this.attacker = attacker;
        this.damager = damager;
        damageType = type;
        if(type == DamageType.Critical)
            this.damage = originDamage = damage * 1.5f;
        else
            this.damage = originDamage = damage;

        attacker.AttackEvent(this);
        damager.HitEvent(this);
    }
}
