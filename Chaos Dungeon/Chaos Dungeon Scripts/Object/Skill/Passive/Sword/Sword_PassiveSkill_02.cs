using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�и� ����
public class Sword_PassiveSkill_02 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isCanParrying = true;
    }
}
