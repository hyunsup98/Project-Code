using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ݷ� ����
public class Sword_PassiveSkill_01 : PassiveSkill
{
    public override void PlayerPassive()
    {
        int damage = 5;
        GameManager.GetPlayer().mobStat.damage += damage;
        GameManager.GetPlayer().passiveDamage = damage;
    }
}
