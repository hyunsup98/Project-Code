using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : WorldUI
{
    [Header("대미지 텍스트 관련 변수")]
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float time = 1.5f;        //텍스트가 생성 후 몇 초 뒤에 지워질지

    private void Start()
    {
        StartCoroutine(Move());
    }

    public override void SetUIData(Transform target, float posY)
    {
        base.SetUIData(target, posY);
    }

    public void SetDamageText(float damage, DamageType type)
    {
        damageText.text = Mathf.Round(damage).ToString();
        
        if(type == DamageType.Critical)
        {
            damageText.fontSize = 0.65f;
            damageText.color = Color.red;
        }
        else
        {
            damageText.fontSize = 0.45f;
            damageText.color = new Color(255, 165, 0);
        }
    }

    private void Take()
    {
        DamageTextManager.Instance.Take(this);
    }

    private IEnumerator Move()
    {
        float timer = time;

        while(timer >= 0)
        {
            posY += 0.002f;
            timer -= Time.deltaTime;

            yield return null;
        }

        yield return new WaitUntil(() => timer < 0);
        Take();
    }

    private void OnEnable()
    {
        StartCoroutine (Move());
        LoadingSceneController.Instance.SceneMoveAction += Take;
    }

    private void OnDisable()
    {
        LoadingSceneController.Instance.SceneMoveAction -= Take;
    }
}
