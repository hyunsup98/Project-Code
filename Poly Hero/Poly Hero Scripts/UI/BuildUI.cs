using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BuildItem
{
    public GameObject prefab;           //���� ��ġ�� ������Ʈ
    public GameObject previewPrefab;    //�̸������ ���� ������Ʈ
}

public class BuildUI : MonoBehaviour
{
    [SerializeField] private GameObject buildUI;

    [SerializeField] private List<GameObject> list_Content = new List<GameObject>();    //������ �������� �����ϴ� â�� ��Ƶδ� ��(����� �̿��� ���� Ű�� ���ؼ�), ��� �ε����� ����ߴ�

    private GameObject buildObject;                //�����ϱ� ���� ������ ������Ʈ
    private GameObject previewObject;              //�̸� ������ ������Ʈ

    public bool isPreviewActivate = false;     //���� �̸����Ⱑ Ȱ��ȭ�Ǿ� �ִ���

    private Transform player;
    private Transform camera;

    //����ĳ��Ʈ�� ���� ����
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    private void Start()
    {
        InitTrans();
    }

    private void Update()
    {
        //Ű���� LŰ�� �̿��� UI�� �Ǵ��״��ϱ�
        if (Input.GetKeyDown(KeyCode.L) && !isPreviewActivate)
            BuildUIOnOff();

        //���� ������ �� ���ӿ� ���� ���� ������Ʈ �����̱�
        if (isPreviewActivate)
            ObjectMove();

        //���� ������ �� ESC�� ������ ����ϱ�
        if (Input.GetKeyDown(KeyCode.Escape))
            BuildCancel();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Build();
    }

    private void InitTrans()
    {
        if(player == null)
            player = GameManager.Instance.player.transform;

        if(camera == null)
            camera = FindObjectOfType<CameraManager>().transform;
    }

    private void Build()
    {
        if (isPreviewActivate && previewObject.GetComponent<Structure>().IsBuildable())
        {
            Instantiate(buildObject, previewObject.transform.position, Quaternion.identity);
            Destroy(previewObject);
            buildObject = null;
            previewObject = null;
            isPreviewActivate = false;
        }
    }

    private void ObjectMove()
    {
        if(Physics.Raycast(camera.position, camera.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 location = hitInfo.point;
                previewObject.transform.position = location;
            }
        }
    }

    private void BuildUIOnOff()
    {
        if (buildUI.activeSelf)
            buildUI.gameObject.SetActive(false);
        else
            buildUI.gameObject.SetActive(true);
    }

    //Ŭ���� ����� index�� �޾ƿ� list_Contect[index]�� content�� ���ش�(SetActive = true)
    public void SwitchBuildTab(int index)
    {
        for (int i = 0; i < list_Content.Count; i++)
        {
            list_Content[i].SetActive(i == index ? true : false);
        }
    }

    //�����ǿ��� ������ ������ Ŭ������ ��
    public void OnClickBuild(BuildSlot build)
    {
        previewObject = Instantiate(build.buildItem.previewPrefab, player.position + player.forward, Quaternion.identity);  //Ŭ���� ���� ������ �̸����� ������Ʈ�� previewObject�� ����
        buildObject = build.buildItem.prefab;   //Ŭ���� ���� ������ ���� ���� ������Ʈ�� buildObject�� ����
        isPreviewActivate = true;               //���� Ȱ��ȭ ����
        buildUI.SetActive(false);               //���� ������ �� �������� ��
    }

    //���� ���¿��� ������� ��
    private void BuildCancel()
    {
        if (isPreviewActivate)
            Destroy(previewObject);

        isPreviewActivate = false;
        previewObject = null;
        buildObject = null;
    }
}
