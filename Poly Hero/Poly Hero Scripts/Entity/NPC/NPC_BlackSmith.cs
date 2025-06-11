using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BlackSmith : NPC
{
    [SerializeField] GameObject forceUI;

    private void Start()
    {
        Init();
    }

    public override void NPCEvent()
    {
        forceUI.SetActive(true);
    }

    protected override void ExitCollider()
    {
        if(forceUI.activeSelf)
        {
            forceUI.SetActive(false);
        }
    }
}
