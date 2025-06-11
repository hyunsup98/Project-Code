using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class EntityStats
{
    public string name;         //�̸�
    public int id;              //���̵�
    public float hp;            //ü��
    public float maxhp;         //�ִ� ü��
    public float speed;         //�̵��ӵ�
    public float damage;        //�����
    public float defence;       //����
    public int level;           //����
}

public abstract class Entity : MonoBehaviour, IDamageable
{
    //��ƼƼ�� ������ ����
    public EntityStats stat = new EntityStats();
    //��ƼƼ�� rigidbody
    [SerializeField] protected Rigidbody rigid;
    //��ƼƼ�� collider
    [SerializeField] protected Collider col;
    //��ƼƼ�� �ִϸ�����
    [SerializeField] protected Animator animator;

    //��ƼƼ�� �ǰݵǾ��� �� ���� �ǰ� ����Ʈ
    [SerializeField] Effect hitEffect;

    protected virtual void Attack()
    {
        
    }

    //���� Ÿ�� ����, ����� ���� ���� ������ �ϴ� ��
    public virtual void AttackEvent(AtkEvent atk)
    {

    }

    //���� �� ����� ���� �� ȿ�� ����
    public abstract void HitEvent(AtkEvent atk);

    //�������� ������� ���� ��
    public virtual void Damage(float damage, Entity attacker, DamageType type)
    {
        stat.hp -= damage;
        Effect e = EffectManager.Instance.Get(hitEffect, transform);
        e.transform.position = transform.position;
        e.GetComponent<ParticleSystem>().Play();
    }
}
