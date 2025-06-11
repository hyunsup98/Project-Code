using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    [Header("보스 체력 관련 변수")]
    [SerializeField] private Image bossHpImg;
    [SerializeField] private TMP_Text bossHpText;
    [SerializeField] private TMP_Text bossInfoText;

    //보스와 첫 조우 시 보스 체력 UI에 체력이 채워지는 연출을 몇 초 동안 할지 정하는 변수
    [SerializeField] private float SetTimer = 2.5f;

    public void SetHP(Entity boss)
    {
        bossHpImg.fillAmount = boss.stat.hp / boss.stat.maxhp;
        bossHpText.text = $"{Mathf.Round(boss.stat.hp)} / {boss.stat.maxhp}";
    }

    public IEnumerator SetBossHpData(Entity boss)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        float timer = 0;
        float maxHp = boss.stat.maxhp;

        while (timer <= SetTimer)
        {
            bossHpImg.fillAmount = Mathf.Lerp(0, 1, timer / SetTimer);
            bossHpText.text = $"{Mathf.Round(Mathf.Lerp(0, maxHp, timer / SetTimer))} / {maxHp}";
            timer += Time.deltaTime;

            yield return null;
        }
    }
}
