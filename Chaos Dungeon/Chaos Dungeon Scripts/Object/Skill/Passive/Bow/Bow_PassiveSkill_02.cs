using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� �ǰ��� �� ���� ����ӵ� ���� (������� �ʱ�ȭ)
public class Bow_PassiveSkill_02 : PassiveSkill
{
    public override void PlayerPassive()
    {
        GameManager.GetPlayer().isComboShot = true;
    }
}
