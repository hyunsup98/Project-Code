using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ä���� �� �ִ� �ڿ�(����, ����)���� Ư�� �������� �� �ǰ� �ϴ� ��ũ��Ʈ
public class CollectionZone : MonoBehaviour
{
    //������ �����ϱ� ���� �ݶ��̴�
    [SerializeField] private Collider colZone;

    //�ش� ������ �����ϰ� �� �ڿ� ������
    [SerializeField] private Collection collect;
    //������ �ڿ��� ���ÿ� ������ �ִ� ����
    [SerializeField] private int maxCount;
    private int count;
    public int Count
    {
        get { return count; }
        set
        {
            if(value < maxCount)
            {
                StartCoroutine(ReCreate(10f));
            }
            count = value;
        }
    }

    //�ڿ��� �ڵ� ������ ����
    private float posX, posZ;

    private void Start()
    {
        posX = colZone.bounds.size.x / 2;
        posZ = colZone.bounds.size.z / 2;

        Count = 0;

        CreateCollection();
    }

    //�ڿ� �����ϱ�
    void CreateCollection()
    {
        if(count < maxCount)
        {
            while(count < maxCount)
            {
                Vector3 pos = SelectPosition();

                Collection coll = CollectManager.Instance.Get(collect, transform);
                coll.transform.SetParent(transform);
                coll.transform.localPosition = pos;
                coll.zone = this;
                Count++;
            }
        }
    }

    Vector3 SelectPosition()
    {
        Vector3 pos;

        float x = Random.Range(-posX, posX);
        float z = Random.Range(-posZ, posZ);

        Collider[] hit = Physics.OverlapBox(new Vector3(x, transform.position.y, z), collect.col.bounds.size, Quaternion.identity, LayerMask.GetMask("Collection"));
        
        if(hit.Length > 0)
        {
            SelectPosition();
        }
        pos = new Vector3(x, transform.position.y, z);

        return pos;
    }

    public IEnumerator ReCreate(float timer)
    {
        yield return new WaitForSeconds(timer);

        CreateCollection();
    }
}
