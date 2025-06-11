using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//방어구 타입
public enum ArmorType
{
    Head,
    Chest,
    Leg
}

//손에 드는 무기(도구) 타입
public enum WeaponType
{
    Sword,
    Axe,
    PickAxe,
    Hand
}

public enum ItemType
{
    Common,     //아무 기능없는 일반 아이템
    HandEquip,  //손에 들 수 있는 무기, 도구
    ArmorEquip, //장착 가능한 방어구
    Use         //사용 가능한 아이템(ex. 포션)
}

public enum ItemGrade
{
    Common,
    Rare,
    Epic,
}

public enum DamageType
{
    Normal,         //일반공격 -> 대미지만 입힘
    Power,          //강공격 -> 상대를 밀쳐냄
    Dot,            //도트공격 -> 일정시간 틱대미지 부여
    Critical        //크리티컬 대미지(기본 대미지의 1.5배 대미지 부여)
}

[System.Serializable]
public class WeaponStats
{
    public WeaponType weapontype;
    public int level = 1;               //강화 가능한 장비들의 레벨
    public int maxLevel = 5;            //강화 가능한 장비들의 최대 레벨
    public float monsterDamage;         //몬스터에게 입히는 대미지
    public float environmentDamage;     //나무, 바위 등에 입히는 대미지
    public float coolTime;              //공격 쿨타임
    public float AttackDelayA;          //대미지가 들어가기 시작하는 타임
    public float AttackDelayB;          //대미지가 들어가지 않기 시작하는 타임
}

[System.Serializable]
public class ItemStats
{
    public int id;
    public string name;
    public string type;     //'재련 강화 재료'같은 아이템의 고유 타입을 작성하는 변수
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

