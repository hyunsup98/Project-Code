using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class MonsterSpawn
{
    public Monster monster;         // ��ȯ�� ����
    public float spawnDelay = 0;        // ������ ��ȯ
    public Transform spawnPosition; // ��ȯ ��ġ
}

public class Wave : DelayBehaviour
{
    public Map map;

    public List<MonsterSpawn> monsters = new List<MonsterSpawn>();

    //���� ���̺꿡 ����ִ� ���� ī��Ʈ
    public int monCount;

    private void Start()
    {
        monCount = monsters.Count;
        foreach (var wave in monsters)
        {
            Delay(() =>
            {

                EffectManager.GetEffect("Spawn", wave.spawnPosition).transform.localScale = new Vector3(0.5f, 0.5f, 0.3f);
                Delay(() =>
                {
                    MonsterManager.Get(wave.monster, wave.spawnPosition).map = map;
                }, 2f);
            }, wave.spawnDelay);
        }
    }

    protected override void Updates() { }
}
