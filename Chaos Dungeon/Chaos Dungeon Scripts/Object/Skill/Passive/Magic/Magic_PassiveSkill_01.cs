using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//마법 대미지 증가
public class Magic_PassiveSkill_01 : PassiveSkill
{
    public float cool = 2f;

    public override void PlayerPassive()
    {
        GameManager.GetPlayer().minusCoolTime = cool;
    }
}
