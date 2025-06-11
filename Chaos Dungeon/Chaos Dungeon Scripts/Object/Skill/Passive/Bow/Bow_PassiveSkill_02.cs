using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적 피격할 때 마다 연사속도 증가 (못맞출시 초기화)
public class Bow_PassiveSkill_02 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isComboShot = true;
    }
}
