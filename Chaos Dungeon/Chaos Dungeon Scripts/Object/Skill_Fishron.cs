using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Fishron : MonoBehaviour
{
    public Boss damager;
    //��ų ��ȯ�� Transform ����Ʈ
    public List<Transform> skillTrans = new List<Transform>();
    //��ȯ�� ��ų ������
    public Skill skill;

    //����ü �ӵ�
    public float speed;

    //ȸ����ų ��
    public int rotateValue;
    bool isRotate = false;
    private void Update()
    {
        if (isRotate)
            transform.Rotate(0, 0, rotateValue * Time.deltaTime);
    }

    IEnumerator Attack()
    {
        if(skillTrans.Count > 0 && skill != null)
        {
            List<Skill> list_Skill = new List<Skill>();

            for (int i = 0; i < skillTrans.Count; i++)
            {
                Skill s = SkillManager.Get(skill, skillTrans[i]);
                s.damager = damager;
                s.speed = 0;
                s.transform.SetParent(skillTrans[i]);
                list_Skill.Add(s);
            }

            isRotate = true;
            yield return new WaitForSeconds(1.5f);

            isRotate = false;
            yield return new WaitForSeconds(0.5f);

            for(int i = 0; i < list_Skill.Count; i++)
            {
                Skill s = list_Skill[i];
                s.transform.rotation = Quaternion.Euler(0, 0, damager.SetLookRotate(s.transform.position));
                s.speed = speed;
                yield return new WaitForSeconds(0.3f);
            }
        }

        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine("Attack");
    }

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
    }
}
