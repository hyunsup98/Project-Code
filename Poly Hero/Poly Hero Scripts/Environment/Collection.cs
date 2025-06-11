using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ä�� ������ �ڿ� ��ũ��Ʈ
public abstract class Collection : MonoBehaviour
{
    public CollectionZone zone;

    [SerializeField] private AudioClip sound;

    [SerializeField] protected Rigidbody rigid;
    public Collider col;

    [SerializeField] protected float hp;
    protected float maxhp;

    [SerializeField] protected ItemDropTable lootItem;

    [SerializeField] Transform dropPoint;   //�������� ����Ǵ� �ʱ� ��ġ
    [SerializeField] float dropTime = 0;    //�ڿ��� ü���� 0�� �ǰ� �� �� �ڿ� �������� ����� ������

    //�ش� �±׸� ������ �ִ� ������ ĳ�� ������ ������� ����
    [SerializeField] WeaponType type;
    private void Start()
    {
        maxhp = hp;
    }

    //ü���� �� ���̸� ������� ���
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

    //��������� �ٽ� ���� �ʱ�ȭ
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
            //ü���� 0 ���ϸ� ������� ����, ���� ä�� ���� �÷��̾� �κ����� ���
            StartCoroutine(dropItems(dropTime));
            SetFalse();
        }
    }

    //time�� �ڿ� �������� ����ϴ� �Լ�
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
