using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Short_Sword : Weapon
{
    public float moveDis;
    // Start is called before the first frame update
    void Start()
    {
        scaleX = 2;
        scaleY = 2;
        scaleZ = 2;
    }

    public override void RightSkill(Vector3 dir, float minusCool)
    {
        base.RightSkill(dir, minusCool);

        float distance = Vector3.Distance(player.transform.position, player.transform.position + dir);

        if(distance > moveDis)
        {
            float rate = moveDis / distance;
            dir = new Vector3(dir.x * rate, dir.y * rate, dir.z);
        }

        player.transform.position += dir;
        player.body2d.velocity = Vector2.zero;
    }
}
