using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : Singleton<MapManager>
{
    [Header("�� ������ ����")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Map startMap;
    [SerializeField] private BossMap lastMap;                       //������ ������
    [SerializeField] private List<Map> mapList = new List<Map>();   //�� �������� ���� ����Ʈ
    [SerializeField] private TransMap transMap;                     //���� ���� ��ġ ������

    private BossMap bossMap;

    public TransMap[,] maps;                //�� �������� �� �迭
    private Queue<Map> queueMap = new Queue<Map>();

    [Header("�� ���� ���� Ŀ���� ���� ����")]
    [SerializeField] private int mapDistance;     //�� ������ �Ÿ�
    [SerializeField] private int maxMapCount;     //���������� �� �� ����
    private int curMapCount = 0;                  //������� ������ ���� ī��Ʈ

    Map checkLastMap = null; // ������ ���� üũ��
    //2���� �迭�� [x, y]��
    public int x, y;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetStartMap();
    }

    //ó�� ������ �� Ÿ�԰� ��ġ�� �����ֱ�
    private void SetStartMap()
    {
        maps = new TransMap[x, y];

        //���� ���� ����
        Map map = Instantiate(startMap);
        map.type = MapType.Start;
        bossMap = Instantiate(lastMap);
        bossMap.type = MapType.Boss;
        bossMap.transform.position = new Vector3(-50, 50, 0);

        //��ü ���� ���� Ȥ�� ���ΰ� 3ĭ �̻��� ��� ��ŸƮ���� ���� �������� �����ϱ�
        int randRow = x > 2 ? Random.Range(1, maps.GetLength(0) - 1) : Random.Range(0, maps.GetLength(0));
        int randCol = y > 2 ? Random.Range(1, maps.GetLength(1) - 1) : Random.Range(0, maps.GetLength(1));

        CreateTransMap(map, randRow, randCol);  //���� ���� TransMap ����
        StructMap(map, randRow, randCol);       //TransMap�� �������� ���� �����ϰ� ���� �ĺ� Ÿ�� ����
        checkLastMap.isLastMap = true;
        map.NextWave();

        GameManager.Map = map;
        GameManager.GetPlayer().transform.position = map.transform.position + map.transform.Find("Ground").GetComponent<TilemapCollider2D>().bounds.center;
        GameManager.Instance.MapCamera();
    }

    //���� ������ �� ���� Ʈ�������� ����
    private void CreateTransMap(Map map, int row, int col)
    {
        TransMap tmap = Instantiate(transMap, transform);
        maps[row, col] = tmap;
        tmap.Set(map, row, col);
        tmap.transform.position = new Vector3(col * mapDistance, -row * mapDistance);

        SetMapInfo(tmap, map);
    }

    //TransMap�� Map�� ������ �־��ֱ�
    private void SetMapInfo(TransMap transMap, Map map)
    {
        transMap.map = map;
        map.transform.SetParent(transMap.transform);
        map.transform.localPosition = Vector3.zero;
        map.transParent = transMap;

        map.name = curMapCount.ToString();
    }

    private void StructMap(Map map, int row, int col)
    {
        //map�� �̵��� �� �ִ� ���� (�� ����Ʈ�� �ִ� ��Ż�� �������� �� 1 ~ list.count��ŭ �� ����)
        List<GameObject> mapPortalList = new List<GameObject>();

        CheckDirection(map, row, col, mapPortalList);

        if (mapPortalList.Count > 0)
        {
            //�� ��Ż�� �� �� �� ������
            int randWayCount = Random.Range(1, mapPortalList.Count + 1);

            for (int i = 0; i < randWayCount; i++)
            {
                curMapCount++;
                if (curMapCount > maxMapCount)
                {
                    OffBool(map, mapPortalList);
                    break;
                }

                //� ��Ż�� �� ������
                int randWay = Random.Range(0, mapPortalList.Count);

                int x = col;
                int y = row;

                switch (mapPortalList[randWay].name)
                {
                    case "UpPortal":
                        y--;
                        break;
                    case "DownPortal":
                        y++;
                        break;
                    case "LeftPortal":
                        x--;
                        break;
                    case "RightPortal":
                        x++;
                        break;
                }
                map.ablePortal.Add(mapPortalList[randWay]);
                SetNearMap(mapPortalList[randWay], y, x);
                mapPortalList.RemoveAt(randWay);
            }
            if(mapPortalList.Count > 0)
                OffBool(map, mapPortalList);
        }
        if (queueMap.Count > 0)
        {
            Map checkMap = queueMap.Dequeue();
            //������ ���� �ƴ� �߸������� �������� �Ǵ°� ����
            if (!checkMap.isOnly)
            {
                checkLastMap = checkMap;
            }
            StructMap(checkMap, checkMap.transParent.row, checkMap.transParent.col);
        }
    }

    private void SetNearMap(GameObject portal, int row, int col)
    {
        List<Map> nearMapList = new List<Map>();

        for (int i = 0; i < mapList.Count; i++)
        {
            switch (portal.name)
            {
                case "UpPortal":
                    SetMap(nearMapList, mapList[i], mapList[i].isDown);
                    break;
                case "DownPortal":
                    SetMap(nearMapList, mapList[i], mapList[i].isUp);
                    break;
                case "LeftPortal":
                    SetMap(nearMapList, mapList[i], mapList[i].isRight);
                    break;
                case "RightPortal":
                    SetMap(nearMapList, mapList[i], mapList[i].isLeft);
                    break;
            }
        }

        int random = Random.Range(0, nearMapList.Count);
        Map nearMap = Instantiate(nearMapList[random]);
        if (nearMap.isOnly)
        {
            mapList.Remove(nearMapList[random]);
        }
        queueMap.Enqueue(nearMap);
        CreateTransMap(nearMap, row, col);
    }

    private void SetMap(List<Map> list, Map map, bool isDir)
    {
        map = map.CheckBool(isDir);
        if (map != null)
        {
            list.Add(map);
        }
    }

    //���� Ư�� ������ ���������� -> �� ���� ��ġ�� Ư�� ������ ���� �ƴϸ� -> ����Ʈ�� �߰�
    
    private void CheckDirection(Map map, int row, int col, List<GameObject> list)
    {
        //isUp
        if (map.isUp && row != 0)
        {
            if (maps[row - 1, col] == null)
                list.Add(map.upPortal);
            else
            {
                if (maps[row - 1, col].map.isDown)
                {
                    map.ablePortal.Add(map.upPortal);
                    ConnectPortal(map.upPortal, maps[row - 1, col].map.downPortal);
                }
                else
                    map.isUp = false;
            }
        }
        else
            map.isUp = false;

        #region �� ���� ���� üũ
        //isDown
        if (map.isDown && row != maps.GetLength(0) - 1)
        {
            if (maps[row + 1, col] == null)
                list.Add(map.downPortal);
            else
            {
                if (maps[row + 1, col].map.isUp)
                {
                    map.ablePortal.Add(map.downPortal);
                    ConnectPortal(map.downPortal, maps[row + 1, col].map.upPortal);
                }
                else
                    map.isDown = false;
            }
        }
        else
            map.isDown = false;


        //isLeft
        if (map.isLeft && col != 0)
        {
            if (maps[row, col - 1] == null)
                list.Add(map.leftPortal);
            else
            {
                if (maps[row, col - 1].map.isRight)
                {
                    map.ablePortal.Add(map.leftPortal);
                    ConnectPortal(map.leftPortal, maps[row, col - 1].map.rightPortal);
                }
                else
                    map.isLeft = false;
            }
        }
        else
            map.isLeft = false;


        //isRight
        if (map.isRight && col != maps.GetLength(1) - 1)
        {
            if (maps[row, col + 1] == null)
                list.Add(map.rightPortal);
            else
            {
                if (maps[row, col + 1].map.isLeft)
                {
                    map.ablePortal.Add(map.rightPortal);
                    ConnectPortal(map.rightPortal, maps[row, col + 1].map.leftPortal);
                }
                else
                    map.isRight = false;
            }
        }
        else
            map.isRight = false;
        #endregion
    }


    //�º��� ��Ż���� ��������
    private void ConnectPortal(GameObject firstPortal, GameObject secondPortal)
    {
        if (!firstPortal.GetComponent<Portal>() && !secondPortal.GetComponent<Portal>())
            return;

        firstPortal.GetComponent<Portal>().nextPortal = secondPortal.GetComponent<Portal>().transform;
        secondPortal.GetComponent<Portal>().nextPortal = firstPortal.GetComponent<Portal>().transform;
    }

    //��Ż ���� ���ǿ� ������ ����Ʈ�� �־����� ������ ���� ������ false�� ����
    private void OffBool(Map map, List<GameObject> list)
    {
        foreach (var item in list)
        {
            switch (item.name)
            {
                case "UpPortal":
                    map.isUp = false;
                    break;
                case "DownPortal":
                    map.isDown = false;
                    break;
                case "LeftPortal":
                    map.isLeft = false;
                    break;
                case "RightPortal":
                    map.isRight = false;
                    break;
            }
        }
    }

    public void EntryLastMap()
    {
        GameManager.GetPlayer().transform.position = bossMap.playerSpawnPos.position;
        GameManager.Map = bossMap;
        GameManager.Instance.MapCamera();
        OffPanel(false);
    }

    public void OffPanel(bool isTrue)
    {
        UIManager.Instance.BossEntryPanel.SetActive(isTrue);
    }
}