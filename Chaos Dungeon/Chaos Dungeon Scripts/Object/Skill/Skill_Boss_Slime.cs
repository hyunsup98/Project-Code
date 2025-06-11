using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Boss_Slime : Skill
{
    protected override void VelocityMove()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(dirX, 1, 0).normalized * speedVelo;
    }
}
