using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//�� �������̽��� ��ӹ����� ������� �Դ� ������Ʈ��
public interface IDamageable
{
    public void Damage(float damage, Entity attacker, DamageType type);
}

//�� �������̽��� ��ӹ����� ��� ������ ��������(ex. ���� ���)
public interface IUseable
{
    public void Use(Entity entity, Slot slot);
}

//�� �������̽��� ��ӹ����� ��ȭ ������ ��������
public interface IForce
{
    public void Force();
}