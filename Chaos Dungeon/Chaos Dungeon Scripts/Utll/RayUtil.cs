using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayUtil
{
    public static Vector3 CheckPlatform(Vector3 vec, Vector3 vec2, float dis)
    {
        RaycastHit2D hit = Physics2D.Raycast(vec, vec2, dis, LayerMask.GetMask("Platform"));
        if (hit.collider != null)
        {
            float disRate = Mathf.Floor((hit.distance * 0.8f) / dis * 100f) / 100f;
            vec2 = new Vector3(vec2.x * disRate, vec2.y * disRate, 0);
        }
        return vec2;
    }
}
