using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterSpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyAI monsterPrefab; //스폰할 몬스터 프리팹
    [SerializeField] private float spawnTime;       //기존 몬스터가 죽고 다시 스폰될 때 까지의 시간

    public EnemyAI monster;                         //현재 스폰되어 있는 몬스터를 할당할 곳

    private void Start()
    {
        Spawn();
    }

    //실질적인 스폰 함수, enemyAI함수에서 실행해줌
    public void SpawnMonster()
    {
        StartCoroutine(CheckSpawn());
    }
    private IEnumerator CheckSpawn()
    {
        yield return new WaitForSeconds(spawnTime);

        if(monster == null)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if(monsterPrefab != null)
        {
            EnemyAI mon = MonsterManager.Instance.Get(monsterPrefab, transform);
            mon.transform.SetParent(transform);
            mon.originPos = transform.position;
            mon.spawnPoint = this;
            monster = mon;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
