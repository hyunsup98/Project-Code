using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_RandomLocMove : AI
{
    [SerializeField] float moveTime;
    [SerializeField] float xSize = 0.3f, ySize = 0.3f;
    [SerializeField] float jumpPower = 5;
    float moveTimer = 0;
    float jump = 0;

    public override bool Run(Monster entity)
    {
        if (moveTimer >= moveTime)
        {
            moveTimer = 0;
            entity.render.flipX = !entity.render.flipX;
        }
        float rotate = entity.render.flipX ? 1 : -1;

        entity.animator.SetTrigger("Move");

        if (entity.IsFly())
        {
            entity.SetVelocity(new Vector2(rotate, jump));
            jump -= GameManager.deltaTime;
            if (jump <= -jumpPower)
            {
                jump = jumpPower;
            }
        }
        else
        {
            entity.SetVelocity(new Vector2(rotate, entity.GetVelocity().y));
            Vector3 pos = entity.transform.position;

            //바닥감지
            //Debug.DrawRay(pos, Vector3.down*ySize, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector3.down, ySize, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (jump < 0.1)
                {
                    jump = 0;
                }
            }

            //전방 벽감지
            int i = (entity.render.flipX ? 1 : -1);
            //Debug.DrawRay(pos, Vector3.right * xSize * i, new Color(0, 1, 0));
            rayHit = Physics2D.Raycast(pos, Vector3.right, xSize * i, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (jump == 0)
                {
                    jump = 0.3f;
                    entity.SetVelocity(new Vector2(rotate, jumpPower));
                }
            }

            if (jump > 0.1)
            {
                jump -= GameManager.deltaTime;
                if (jump <= 0) jump = 0.05f;
            }
        }
        moveTimer += GameManager.deltaTime;
        return true;
    }
}
