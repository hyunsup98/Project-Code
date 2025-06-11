using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkEvent
{
    public Entity attacker;           //���� ���� ���
    public Entity damager;            //���� ���� ���
    public float damage;              //�����
    public float originDamage;        //���� �������� ���� �����
    public DamageType damageType;     //����� Ÿ��

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
