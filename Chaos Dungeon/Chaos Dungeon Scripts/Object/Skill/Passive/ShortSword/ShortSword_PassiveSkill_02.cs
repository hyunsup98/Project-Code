using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ν�
public class ShortSword_PassiveSkill_02 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isCorrode = true;
    }
}
