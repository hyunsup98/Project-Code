using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Healer : Npc
{
    [SerializeField] bool isHeal = false;
    protected override void InitStart()
    {

    }

    //강화창 보여주기
    public override void NPCEvent()
    {
        if (!isHeal)
        {
            Player p = GameManager.GetPlayer();
            if(p.mobStat.max_hp > p.mobStat.hp)
            {
                isHeal = true;
                p.mobStat.hp += p.mobStat.max_hp * 0.3f;
                p.hpBar.SetHp(p.mobStat.hp / p.mobStat.max_hp);
                if(p.mobStat.hp > p.mobStat.max_hp)
                {
                    p.mobStat.hp = p.mobStat.max_hp;
                }
            }
        }
    }
}
