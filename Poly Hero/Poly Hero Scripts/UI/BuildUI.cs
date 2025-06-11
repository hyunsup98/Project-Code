using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BuildItem
{
    public GameObject prefab;           //실제 설치될 오브젝트
    public GameObject previewPrefab;    //미리보기로 보일 오브젝트
}

public class BuildUI : MonoBehaviour
{
    [SerializeField] private GameObject buildUI;

    [SerializeField] private List<GameObject> list_Content = new List<GameObject>();    //건축할 아이템을 선택하는 창을 모아두는 곳(토글을 이용해 끄고 키기 위해서), 토글 인덱스와 맞춰야댐

    private GameObject buildObject;                //건축하기 위해 선택한 오브젝트
    private GameObject previewObject;              //미리 보여질 오브젝트

    public bool isPreviewActivate = false;     //건축 미리보기가 활성화되어 있는지

    private Transform player;
    private Transform camera;

    //레이캐스트를 위한 변수
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float range;

    private void Start()
    {
        InitTrans();
    }

    private void Update()
    {
        //키보드 L키를 이용해 UI를 컸다켰다하기
        if (Input.GetKeyDown(KeyCode.L) && !isPreviewActivate)
            BuildUIOnOff();

        //건축 상태일 때 에임에 따라 건축 오브젝트 움직이기
        if (isPreviewActivate)
            ObjectMove();

        //건축 상태일 때 ESC를 누르면 취소하기
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

    //클릭한 토글의 index를 받아와 list_Contect[index]의 content만 켜준다(SetActive = true)
    public void SwitchBuildTab(int index)
    {
        for (int i = 0; i < list_Content.Count; i++)
        {
            list_Content[i].SetActive(i == index ? true : false);
        }
    }

    //건축탭에서 건축할 슬롯을 클릭했을 때
    public void OnClickBuild(BuildSlot build)
    {
        previewObject = Instantiate(build.buildItem.previewPrefab, player.position + player.forward, Quaternion.identity);  //클릭한 건축 슬롯의 미리보기 오브젝트를 previewObject에 넣음
        buildObject = build.buildItem.prefab;   //클릭한 건축 슬롯의 실제 건축 오브젝트를 buildObject에 넣음
        isPreviewActivate = true;               //건축 활성화 상태
        buildUI.SetActive(false);               //건축 상태일 때 건축탭을 끔
    }

    //건축 상태에서 취소했을 때
    private void BuildCancel()
    {
        if (isPreviewActivate)
            Destroy(previewObject);

        isPreviewActivate = false;
        previewObject = null;
        buildObject = null;
    }
}
