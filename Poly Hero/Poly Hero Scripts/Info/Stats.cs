using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� Ÿ��
public enum ArmorType
{
    Head,
    Chest,
    Leg
}

//�տ� ��� ����(����) Ÿ��
public enum WeaponType
{
    Sword,
    Axe,
    PickAxe,
    Hand
}

public enum ItemType
{
    Common,     //�ƹ� ��ɾ��� �Ϲ� ������
    HandEquip,  //�տ� �� �� �ִ� ����, ����
    ArmorEquip, //���� ������ ��
    Use         //��� ������ ������(ex. ����)
}

public enum ItemGrade
{
    Common,
    Rare,
    Epic,
}

public enum DamageType
{
    Normal,         //�Ϲݰ��� -> ������� ����
    Power,          //������ -> ��븦 ���ĳ�
    Dot,            //��Ʈ���� -> �����ð� ƽ����� �ο�
    Critical        //ũ��Ƽ�� �����(�⺻ ������� 1.5�� ����� �ο�)
}

[System.Serializable]
public class WeaponStats
{
    public WeaponType weapontype;
    public int level = 1;               //��ȭ ������ ������ ����
    public int maxLevel = 5;            //��ȭ ������ ������ �ִ� ����
    public float monsterDamage;         //���Ϳ��� ������ �����
    public float environmentDamage;     //����, ���� � ������ �����
    public float coolTime;              //���� ��Ÿ��
    public float AttackDelayA;          //������� ���� �����ϴ� Ÿ��
    public float AttackDelayB;          //������� ���� �ʱ� �����ϴ� Ÿ��
}

[System.Serializable]
public class ItemStats
{
    public int id;
    public string name;
    public string type;     //'��� ��ȭ ���'���� �������� ���� Ÿ���� �ۼ��ϴ� ����
    public ItemType itemType;
    public ItemGrade grade;
    [Multiline]
    public string description;
    public int buyPrice;
    public int sellPrice;
    public int maxStack;
}

[System.Serializable]
public class NpcData
{
    public int id;
    public string name;
    public List<string> lines = new List<string>();
}

