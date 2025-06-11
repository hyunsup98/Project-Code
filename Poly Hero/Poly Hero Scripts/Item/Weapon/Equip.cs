using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;



public abstract class Equip : Item, IForce
{
    public ForceTable forceTable;

    [SerializeField] float posX, posY, posZ;
    [SerializeField] float rotX, rotY, rotZ;
    //true�� ������� ���� ����
    public bool isDamage;
    public PlayerController player;

    [Space]
    //���� ����
    public WeaponStats stats;

    //�ߺ� ������ �����ϱ� ���� ����Ʈ
    List<Entity> hitList = new List<Entity>();

    // Start is called before the first frame update
    void Start()
    {
        isDamage = false;
        stats.level = 1;
    }

    public void SetPosition()
    {
        transform.localPosition = new Vector3(posX, posY, posZ);
        transform.localRotation = Quaternion.Euler(rotX, rotY, rotZ);
    }

    public void ResetHitList()
    {
        hitList.Clear();
    }

    //������� ���� ���·� ������� (�ִϸ��̼� �̺�Ʈ�� �������� �Լ�)
    public void AttackReady()
    {
        ResetHitList();
        isDamage = true;
    }

    public void Force()
    {
        stats.level++;
    }
}
