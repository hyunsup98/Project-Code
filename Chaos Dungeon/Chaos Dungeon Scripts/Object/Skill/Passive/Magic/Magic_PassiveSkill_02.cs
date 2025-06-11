using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Èú ½ºÅ³ º¸À¯
public class Magic_PassiveSkill_02 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isHeal = true;
    }
}
