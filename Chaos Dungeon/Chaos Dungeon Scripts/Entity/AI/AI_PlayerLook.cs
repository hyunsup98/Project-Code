using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



// �÷��̾ max �ȿ� �ִٸ� �÷��̾ �ٶ�

public class AI_PlayerLook : AI
{
    [SerializeField] float range;


    public override bool Run(Monster entity)
    {
        Vector2 ploc = GameManager.GetPlayer().transform.position;
        Vector2 eloc = entity.transform.position;
        float range = Vector3.Distance(eloc, ploc);
        if (range <= this.range)
        {
            Vector2 v = (ploc - eloc).normalized;
            entity.lookRotate = Mathf.Rad2Deg * (Mathf.Atan2(v.y, v.x));
        }
        return true;
    }
}
