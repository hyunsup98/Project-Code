using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//채집할 수 있는 자원(나무, 바위)등이 특정 구역에서 젠 되게 하는 스크립트
public class CollectionZone : MonoBehaviour
{
    //범위를 지정하기 위한 콜라이더
    [SerializeField] private Collider colZone;

    //해당 존에서 생성하게 될 자원 프리팹
    [SerializeField] private Collection collect;
    //존에서 자원을 동시에 생성할 최대 개수
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

    //자원을 자동 생성할 범위
    private float posX, posZ;

    private void Start()
    {
        posX = colZone.bounds.size.x / 2;
        posZ = colZone.bounds.size.z / 2;

        Count = 0;

        CreateCollection();
    }

    //자원 생성하기
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
