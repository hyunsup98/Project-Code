using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : DelayBehaviour
{
    [SerializeField] float time;
    protected float timer = 0;

    private void OnEnable()
    {
        timer = time;
        StartSet();
    }

    // Update is called once per frame
    protected override void Updates()
    {
        Run();

        timer -= GameManager.deltaTime;
        if(timer <= 0)
        {
            EffectManager.Remove(this);
        }
    }

    protected abstract void StartSet();
    protected abstract void Run();
}
