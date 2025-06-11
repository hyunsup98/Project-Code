using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityUIManager : Singleton<EntityUIManager>
{
    [SerializeField] private HPBarUI hpBar;
    [SerializeField] private DamageText damageText;
    [SerializeField] private NpcUI npcUI;
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
    }

    public HPBarUI CreateHpBar(Transform target, float posY)
    {
        HPBarUI hp = HPBarManager.Instance.Get(hpBar, transform);
        hp.SetUIData(target, posY);
        return hp;
    }

    public DamageText CreateDamageText(Transform target, float posY, float damage, DamageType type)
    {
        DamageText dText = DamageTextManager.Instance.Get(damageText, transform);
        dText.SetUIData(target, posY);
        dText.SetDamageText(damage, type);
        return dText;
    }

    public NpcUI CreateNpcUI(NPC target, float posY)
    {
        NpcUI npc = NpcUIManager.Instance.Get(npcUI, transform);
        npc.SetUIData(target.transform, posY);
        npc.SetNpcInfo(target);
        return npc;
    }
}
