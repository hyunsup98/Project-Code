using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Bow : Skill
{
    //ȭ���� right������ velocity�� ��� ���󰡴� ������ �ٶ󺸵��� ��
    protected override void Move()
    {
        transform.right = GetComponent<Rigidbody2D>().velocity;
    }

    protected override void VelocityMove()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * 4f;
    }
}
