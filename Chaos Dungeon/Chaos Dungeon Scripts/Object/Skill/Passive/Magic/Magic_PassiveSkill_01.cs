using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ����� ����
public class Magic_PassiveSkill_01 : PassiveSkill
{
    public float cool = 2f;

    public override void PlayerPassive()
    {
        GameManager.GetPlayer().minusCoolTime = cool;
    }
}
