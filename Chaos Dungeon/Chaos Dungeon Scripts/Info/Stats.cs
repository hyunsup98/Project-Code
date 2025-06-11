using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MobStats
{
    public float hp;
    public float max_hp;
    public float damage;
    public float defence;
    public float origin_damage;
    public float move_speed;
    public float corrode;           //�ν�(���) ��ġ
    public float bleedDamage;       //���� �����
}
public enum ItemType
{
    SWORD,
    SHORT_SWORD,
    BOW,
    MAGIC,
    ACCESSORY,
    ETC
}

[System.Serializable]
public enum SlotType
{
    EQUIPWEAPON,        
    EQUIPACCESSORY,     
    INVEN,
    REINFORCE
}

[System.Serializable]
public class ItemStats
{
    public int id;
    public string name;
    public float damage;
    public int maxStack;
    public int price;
    public ItemType type;
    public ItemGrade grade;
    public Sprite item_img;
    [Multiline]
    public string lore;
}

public enum ItemGrade
{
    COMMON, //���
    NORMAL, //�ʷ�
    RARE,   //�Ķ�
    EPIC,   //����
    LEGEND  //���
}

[System.Serializable]
public class NpcData
{
    public int id;
    public string name;
    public List<string> lines = new();
}