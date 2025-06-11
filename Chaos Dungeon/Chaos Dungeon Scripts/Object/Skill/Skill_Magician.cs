using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_Magician : MonoBehaviour
{
    //스킬 시전하는 엔티티
    public Boss damager;
    //스킬이 발사될 트랜스폼
    public List<Transform> skillTrans = new List<Transform>();
    public Skill bullet;
    public float rotZ;
    public int count;

    IEnumerator Shoot()
    {
        float z = 0;
        float rotate = 360 / skillTrans.Count;

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < skillTrans.Count; j++)
            {
                Skill s = SkillManager.Get(bullet, skillTrans[j]);
                s.damager = damager;
                s.start_rotate = (rotate * j) + z;
                s.transform.SetParent(SkillManager.Instance.transform);
            }
            z += rotZ;
            transform.rotation = Quaternion.Euler(0, 0, z);

            yield return new WaitForSeconds(0.2f);
        }
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine("Shoot");
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
    }
}
