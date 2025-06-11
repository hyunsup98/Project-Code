using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class BossEntry : MonoBehaviour
{
    [SerializeField] private Entity boss;

    [Header("컷신을 실행하기 위한 변수")]
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
