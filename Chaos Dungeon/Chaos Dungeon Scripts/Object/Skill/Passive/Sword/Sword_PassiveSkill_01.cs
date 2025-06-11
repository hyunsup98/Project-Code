using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공격력 증가
public class Sword_PassiveSkill_01 : PassiveSkill
{
    public override void PlayerPassive()
    {
        int damage = 5;
        GameManager.GetPlayer().mobStat.damage += damage;
        GameManager.GetPlayer().passiveDamage = damage;
    }
}
