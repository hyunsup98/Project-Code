using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : ObjectPoolGroup<Monster>
{

    //�ӽ÷� transform
    //�����ġ���� ���� ����� ���͸� ������
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