using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ÃâÇ÷
public class ShortSword_PassiveSkill_01 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isBleedAttack = true;
    }
}
