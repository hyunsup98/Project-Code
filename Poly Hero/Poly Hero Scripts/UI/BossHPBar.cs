using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    [Header("���� ü�� ���� ����")]
    [SerializeField] private Image bossHpImg;
    [SerializeField] private TMP_Text bossHpText;
    [SerializeField] private TMP_Text bossInfoText;

    //������ ù ���� �� ���� ü�� UI�� ü���� ä������ ������ �� �� ���� ���� ���ϴ� ����
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
