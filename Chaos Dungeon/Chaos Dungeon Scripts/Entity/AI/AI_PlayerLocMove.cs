using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



// 플레이어가 max_range 안에 있으면 min_range가 될때까지 플레이어에게 이동

public class AI_PlayerLocMove : AI
{
    [SerializeField] float max_range;
    [SerializeField] float min_range;
    [SerializeField] float ySize = 0.3f;

    float jump = 0;

    public override bool Run(Monster entity)
    {
        Vector2 ploc = GameManager.GetPlayer().transform.position;
        Vector2 eloc = entity.transform.position;
        float range = Vector2.Distance(eloc, ploc);

        if (!entity.IsFly())
        {
            if (jump > 0.1)
            {
                jump -= GameManager.deltaTime;
                if (jump <= 0) jump = 0.05f;
            }

            Vector3 pos = entity.transform.position;

            //바닥감지
            //Debug.DrawRay(pos, Vector3.down * ySize, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector3.down, ySize, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (jump < 0.1)
                {
                    jump = 0;
                }
            }
        }
        if (range < max_range && range > min_range) {
            Vector2 move = ploc - eloc;
            entity.animator.SetTrigger("Move");
            if (entity.IsFly())
            {
                entity.SetVelocity(move.normalized);
            }
            else if (jump <= 0)
            {
                float movey = GameManager.GetPlayer().GetVelocity().y;
                move.y = movey > move.y*10 ? movey : move.y*10;
                if (move.y > 10) move.y = 10;

                entity.SetVelocity(new Vector2(move.normalized.x, (move.y > 0 ? move.y : 0)));
                jump = 0.3f;
            }
            else
            {
                entity.SetVelocity(new Vector2(move.normalized.x, entity.GetVelocity().y));
            }
            return false;
        }
        return true;
    }
}
