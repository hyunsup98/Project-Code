using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public float posY;

    private List<Collider> colliderList = new List<Collider>(); //���� �Ұ����� ������Ʈ�� �ݶ��̴��� �ε����� ��� colliderList�� �߰�

    [SerializeField] private LayerMask ignoreLayer; //ignoreLayer�� �ش��ϴ� ������Ʈ�� colliderList���� ���ܵȴ�.

    [SerializeField] private Renderer structureRenderer;     //���׸����� �ٲٱ� ���� renderer�� �޾ƿ�
    [SerializeField] private Material green;        //���� ������ ������ �� ������ Material => ColliderList�� ������ 0 �� ��
    [SerializeField] private Material red;          //���� �Ұ����� ������ �� ������ Material => ColliderList�� ������ 1 �̻��� ��

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

    //���� ��ȯ: colliderList�� ������ 0�̸� ���� ���� ����, 0�� �ƴϸ�(1�� �̻��̸�) ���� �Ұ��� ����
    public bool IsBuildable()
    {
        return colliderList.Count == 0 ? true : false;
    }
}
