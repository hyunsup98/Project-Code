using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//거리 비례 대미지
public class Bow_PassiveSkill_01 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isLongShot = true;
    }
}
