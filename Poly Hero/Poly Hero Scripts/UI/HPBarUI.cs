using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : WorldUI
{
    [SerializeField] private Image hpImg;             //체력바 이미지
    [SerializeField] private TMP_Text monsterInfoTxt; //몬스터 이름, 레벨 텍스트

    private EnemyAI enemy;

    public override void SetUIData(Transform target, float posY)
    {
        base.SetUIData(target, posY);

        //target이 EnemyAI 클래스를 가지고 있으면 UI 세팅
        if(target.GetComponent<EnemyAI>() != null )
        {
            enemy = target.GetComponent<EnemyAI>();
            monsterInfoTxt.text = $"LV:{enemy.stat.level} <color=green>{enemy.stat.name}</color>";
            SetFillAmount();
            enemy.HitAction += SetFillAmount;
        }
    }

    //체력바 갱신
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
