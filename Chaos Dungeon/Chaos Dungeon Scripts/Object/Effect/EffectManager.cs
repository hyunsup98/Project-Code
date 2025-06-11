using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : ObjectPoolGroup<Effect>
{
    [SerializeField] public List<Effect> effects;

    public static Effect GetEffect(string name, Transform local)
    {
        foreach(Effect ef in Instance.GetComponent<EffectManager>().effects)
        {
            if(ef.name == name)
            {
                return Get(ef, local);
            }
        }
        return null;
    }
}
