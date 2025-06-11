using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Bow : Skill
{
    //화살의 right방향이 velocity에 담긴 날라가는 방향을 바라보도록 함
    protected override void Move()
    {
        transform.right = GetComponent<Rigidbody2D>().velocity;
    }

    protected override void VelocityMove()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * 4f;
    }
}
