using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_BlackSmith : Npc
{
    ReinforceUI reinforceUI;

    protected override void InitStart()
    {
        reinforceUI = UIManager.Instance.reinforceUI;
    }

    //강화창 보여주기
    public override void NPCEvent()
    {
        reinforceUI.gameObject.SetActive(true);
    }
}
