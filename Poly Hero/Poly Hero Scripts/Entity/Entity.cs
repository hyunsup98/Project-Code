using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class EntityStats
{
    public string name;         //이름
    public int id;              //아이디
    public float hp;            //체력
    public float maxhp;         //최대 체력
    public float speed;         //이동속도
    public float damage;        //대미지
    public float defence;       //방어력
    public int level;           //레벨
}

public abstract class Entity : MonoBehaviour, IDamageable
{
    //엔티티가 가지는 스탯
    public EntityStats stat = new EntityStats();
    //엔티티의 rigidbody
    [SerializeField] protected Rigidbody rigid;
    //엔티티의 collider
    [SerializeField] protected Collider col;
    //엔티티의 애니메이터
    [SerializeField] protected Animator animator;

    //엔티티가 피격되었을 때 나올 피격 이펙트
    [SerializeField] Effect hitEffect;

    protected virtual void Attack()
    {
        
    }

    //공격 타입 변경, 대미지 증가 등의 연산을 하는 곳
    public virtual void AttackEvent(AtkEvent atk)
    {

    }

    //맞을 때 대미지 감소 등 효과 구현
    public abstract void HitEvent(AtkEvent atk);

    //실질적인 대미지를 입을 때
    public virtual void Damage(float damage, Entity attacker, DamageType type)
    {
        stat.hp -= damage;
        Effect e = EffectManager.Instance.Get(hitEffect, transform);
        e.transform.position = transform.position;
        e.GetComponent<ParticleSystem>().Play();
    }
}
