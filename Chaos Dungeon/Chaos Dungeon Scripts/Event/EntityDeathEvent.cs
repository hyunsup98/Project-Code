using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����� ������ �̺�Ʈ �߻� 
//�����ѳ� , �ǰݴ��

public class EntityDeathEvent
{
    bool isCancel = false;

    Entity killer;
    Entity target;


    public EntityDeathEvent(Entity killer,Entity target)
    {
        this.killer = killer;
        this.target = target;
    }

    public bool Cancel
    {
        get { return isCancel; }
        set { isCancel = value; }
    }


    public Entity GetTarget()
    {
        return this.target;
    }

    public Entity GetKiller()
    {
        return this.killer;
    }
}
