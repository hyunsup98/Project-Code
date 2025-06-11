using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public float posY;

    private List<Collider> colliderList = new List<Collider>(); //건축 불가능한 오브젝트의 콜라이더와 부딪혔을 경우 colliderList에 추가

    [SerializeField] private LayerMask ignoreLayer; //ignoreLayer에 해당하는 오브젝트는 colliderList에서 제외된다.

    [SerializeField] private Renderer structureRenderer;     //메테리얼을 바꾸기 위해 renderer를 받아옴
    [SerializeField] private Material green;        //건축 가능한 상태일 때 적용할 Material => ColliderList의 개수가 0 일 때
    [SerializeField] private Material red;          //건축 불가능한 상태일 때 적용할 Material => ColliderList의 개수가 1 이상일 때

    private void SetMaterial()
    {
        if(colliderList.Count > 0)
        {
            structureRenderer.material = red;
        }
        else
        {
            structureRenderer.material = green;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != ignoreLayer )
        {
            colliderList.Add(other);
            SetMaterial();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != ignoreLayer)
        {
            colliderList.Remove(other);
            SetMaterial();
        }
    }

    //상태 반환: colliderList의 개수가 0이면 건축 가능 상태, 0이 아니면(1개 이상이면) 건축 불가능 상태
    public bool IsBuildable()
    {
        return colliderList.Count == 0 ? true : false;
    }
}
