using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : Effect
{
    [SerializeField] SpriteRenderer front;
    [SerializeField] SpriteRenderer back;
    float a = 0, a2 = 0;

    protected override void StartSet()
    {
        a = 0;
        a2 = 0;
    }

    protected override void Run()
    {
        front.color = new Color(front.color.r, front.color.g, front.color.b, a);
        back.color = new Color(back.color.r, back.color.g, back.color.b, a2);

        if (a > 0.5)
        {
            a2 += Time.deltaTime;

            front.transform.Rotate(new Vector3(0, 0, Time.deltaTime * 30 * a2));
        }
        if (timer <= 1)
        {
            back.transform.Rotate(new Vector3(0, 0, Time.deltaTime * -30 * a2));
            a -= Time.deltaTime * 2;
            a2 -= Time.deltaTime * 2;
        } else if (a < 0.7)
        {
            a += Time.deltaTime;
        }
    }
}
