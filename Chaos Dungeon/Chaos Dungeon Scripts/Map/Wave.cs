using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class MonsterSpawn
{
    public Monster monster;         // 소환할 몬스터
    public float spawnDelay = 0;        // 몇초후 소환
    public Transform spawnPosition; // 소환 위치
}

public class Wave : DelayBehaviour
{
    public Map map;

    public List<MonsterSpawn> monsters = new List<MonsterSpawn>();

    //현재 웨이브에 살아있는 몬스터 카운트
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
