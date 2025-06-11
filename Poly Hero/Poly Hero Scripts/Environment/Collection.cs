using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//채집 가능한 자원 스크립트
public abstract class Collection : MonoBehaviour
{
    public CollectionZone zone;

    [SerializeField] private AudioClip sound;

    [SerializeField] protected Rigidbody rigid;
    public Collider col;

    [SerializeField] protected float hp;
    protected float maxhp;

    [SerializeField] protected ItemDropTable lootItem;

    [SerializeField] Transform dropPoint;   //아이템이 드랍되는 초기 위치
    [SerializeField] float dropTime = 0;    //자원이 체력이 0이 되고 몇 초 뒤에 아이템을 드랍할 것인지

    //해당 태그를 가지고 있는 도구로 캐야 온전히 대미지를 받음
    [SerializeField] WeaponType type;
    private void Start()
    {
        maxhp = hp;
    }

    //체력이 다 깎이면 사라지는 모션
    protected virtual void SetFalse()
    {
        rigid.isKinematic = false;
        rigid.useGravity = true;

        StartCoroutine("DestroyObject");
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(3f);
        zone.Count--;
        CollectManager.Instance.Take(this);
    }

    //사라지고나서 다시 세팅 초기화
    protected virtual void SetData()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        rigid.isKinematic = true;
        rigid.useGravity = false;
        hp = maxhp;
    }

    public void Damage(float damage)
    {
        hp -= damage;
        SoundManager.Instance.SetSound(sound, transform);
        if (hp <= 0)
        {
            //체력이 0 이하면 사라지는 동작, 많은 채집 보상 플레이어 인벤으로 드랍
            StartCoroutine(dropItems(dropTime));
            SetFalse();
        }
    }

    //time초 뒤에 아이템을 드랍하는 함수
    IEnumerator dropItems(float time)
    {
        yield return new WaitForSeconds(time);

        lootItem.DropItems(dropPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Equip>())
        {
            if (other.GetComponent<Equip>().stats.weapontype == type && other.GetComponent<Equip>().isDamage)
            {
                other.GetComponent<Equip>().isDamage = false;
                Damage(other.GetComponent<Equip>().stats.environmentDamage);
            }
        }
    }

    private void OnDisable()
    {
        SetData();
    }
}
