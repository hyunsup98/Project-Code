using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : WorldUI
{
    [SerializeField] private Image hpImg;             //ü�¹� �̹���
    [SerializeField] private TMP_Text monsterInfoTxt; //���� �̸�, ���� �ؽ�Ʈ

    private EnemyAI enemy;

    public override void SetUIData(Transform target, float posY)
    {
        base.SetUIData(target, posY);

        //target�� EnemyAI Ŭ������ ������ ������ UI ����
        if(target.GetComponent<EnemyAI>() != null )
        {
            enemy = target.GetComponent<EnemyAI>();
            monsterInfoTxt.text = $"LV:{enemy.stat.level} <color=green>{enemy.stat.name}</color>";
            SetFillAmount();
            enemy.HitAction += SetFillAmount;
        }
    }

    //ü�¹� ����
    private void SetFillAmount()
    {
        hpImg.fillAmount = enemy.stat.hp / enemy.stat.maxhp;
    }

    private void Take()
    {
        HPBarManager.Instance.Take(this);
    }

    private void OnEnable()
    {
        if(LoadingSceneController.Instance != null)
        {
            LoadingSceneController.Instance.SceneMoveAction += Take;
        }
    }

    private void OnDisable()
    {
        if(enemy != null)
        {
            enemy.HitAction -= SetFillAmount;
        }

        if (LoadingSceneController.Instance != null)
        {
            LoadingSceneController.Instance.SceneMoveAction -= Take;
        }
    }
}
