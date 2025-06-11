using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ���� ���̽�

public abstract class AI : MonoBehaviour
{
    public abstract bool Run(Monster entity);

    public virtual void DamageEvent(EntityDamageEvent evt)
    {

    }

    public virtual void AttackEvent(EntityDamageEvent evt)
    {

    }

}
