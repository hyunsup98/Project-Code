using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� ��ų ����
public class Magic_PassiveSkill_02 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isHeal = true;
    }
}
