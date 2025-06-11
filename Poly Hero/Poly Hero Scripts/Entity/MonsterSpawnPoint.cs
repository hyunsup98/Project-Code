using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterSpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyAI monsterPrefab; //������ ���� ������
    [SerializeField] private float spawnTime;       //���� ���Ͱ� �װ� �ٽ� ������ �� ������ �ð�

    public EnemyAI monster;                         //���� �����Ǿ� �ִ� ���͸� �Ҵ��� ��

    private void Start()
    {
        Spawn();
    }

    //�������� ���� �Լ�, enemyAI�Լ����� ��������
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
