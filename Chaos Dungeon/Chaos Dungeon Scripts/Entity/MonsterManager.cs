using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : ObjectPoolGroup<Monster>
{

    //임시로 transform
    //대상위치에서 가장 가까운 몬스터를 가져옴
    public Transform GetTarget(Vector2 loc)
    {
        Transform target = null;
        float min = 99999;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            float range = Vector2.Distance(child.position, loc);
            if (range < min)
            {
                //if (child.GetComponent<Monster>().isAlive)
                {
                    min = range;
                    target = child;
                }
            }
        }
        if (target != null)
        {
            return target;
        }
        return null;
    }

    static public int GetSpawnCount()
    {
        int c = 0;
        for (int i = 0; i < Instance.transform.childCount; i++)
        {
            Transform child = Instance.transform.GetChild(i);
            if (child.gameObject.active)
            {
                c++;
            }
        }
        return c;
    }
}