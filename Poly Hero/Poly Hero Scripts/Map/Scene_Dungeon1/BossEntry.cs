using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossEntry : MonoBehaviour
{
    [SerializeField] private Entity boss;

    [Header("�ƽ��� �����ϱ� ���� ����")]
    [SerializeField] private PlayableDirector pd;
    [SerializeField] private TimelineAsset taBossEntry;
    [SerializeField] private Collider col;


    public void SetBossHp()
    {
        StartCoroutine(UIManager.Instance.bossHpBar.SetBossHpData(boss));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            col.enabled = false;
            pd.Play();
        }
    }
}
