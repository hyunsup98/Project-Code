using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ºÎ½Ä
public class ShortSword_PassiveSkill_02 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isCorrode = true;
    }
}
