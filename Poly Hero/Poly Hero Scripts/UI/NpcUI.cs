using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcUI : WorldUI
{
    [SerializeField] private TMP_Text npc_Text;

    public override void SetUIData(Transform target, float posY)
    {
        base.SetUIData(target, posY);
    }

    public void SetNpcInfo(NPC npc)
    {
        npc_Text.text = $"<{npc.npcName}>";
        npc_Text.color = npc.uiColor;
    }

    private void Take()
    {
        NpcUIManager.Instance.Take(this);
    }

    private void OnEnable()
    {
        LoadingSceneController.Instance.SceneMoveAction += Take;
    }

    private void OnDisable()
    {
        LoadingSceneController.Instance.SceneMoveAction -= Take;
    }
}
