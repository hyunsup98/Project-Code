using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����� �ٸ� ��󿡰� ���ظ� ������ �߻�
//�����ѳ� , �ǰݴ�� , ����

//����Ÿ�� �Ϲ� , ���� , �������� �켱 �з��ص�
public enum AttackType
{
    NORMAL,
    DOT,        //��Ʈ & ����
    FIXED,
    COR         //�ν�(���)
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
