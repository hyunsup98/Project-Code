using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Ÿ� ��� �����
public class Bow_PassiveSkill_01 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isLongShot = true;
    }
}
