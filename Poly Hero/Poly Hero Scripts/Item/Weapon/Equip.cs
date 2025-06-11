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
    //true면 대미지가 들어가는 상태
    public bool isDamage;
    public PlayerController player;

    [Space]
    //무기 스탯
    public WeaponStats stats;

    //중복 공격을 방지하기 위한 리스트
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

    //대미지가 들어가는 상태로 만들어줌 (애니메이션 이벤트로 지정해줄 함수)
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
