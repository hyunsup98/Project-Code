# 포트폴리오 코드를 정리해놓은 Repository 입니다.

**아래의 주요 기능 코드 외에도 전체 코드가 담겨져있습니다.**

<br><br><br>

# 목차

## 1. [Chaos Dungeon](https://github.com/hyunsup98/Project-Code/edit/main/README.md#13)

## 2. [Poly Hero](https://github.com/hyunsup98/Project-Code/edit/main/README.md#)

# 0. 카오스 던전

- **프로젝트 소개**
  - 2D 횡스크롤뷰 로그라이크 게임입니다.
    
    총 3개의 스테이지로 이루어져있습니다.
  
    패시브 스킬을 정하고 여러 무기들을 활용하여 몬스터를 물리치고 마지막 보스를 잡아보세요.


## 0-1 주요 기능 코드

- **오브젝트 풀**
  - 경로: *Chaos Dungeon/Chaos Dungeon Scripts/Util/ObjectPool.cs*
  - 오브젝트의 효율적인 관리를 위해 오브젝트 풀링 기법을 사용하였습니다.
  - 오브젝트 풀링을 사용하는 객체가 여러개이므로 제네릭을 활용해 커스텀 ObjectPool 스크립트를 작성했습니다.

 <br>
 
```ObjectPool.cs
  public class ObjectPool<T> : Singleton<ObjectPool<T>> where T: MonoBehaviour
{
    protected Queue<T> objects = new Queue<T>();

    public static Queue<T> GetQueue()
    {
        return Instance.objects;
    }

    public static T Get(T type, Transform pos)
    {
        string name = type.name;
        Queue<T> objq = Instance.objects;

        if (objq.Count <= 0)
        {
            T o = Instantiate(type, pos);
            o.name = name;
            objq.Enqueue(o);
        }

        type = objq.Dequeue();
        type.transform.position = pos.position;
        type.gameObject.SetActive(true);

        return type;
    }

    public static void Remove(T obj)
    {
        string name = obj.name;
        obj.gameObject.SetActive(false);
        Instance.objects.Enqueue(obj);
    }
}
```
<br><br><br>

- **동적 맵 생성 알고리즘**
  - 경로: *Chaos Dungeon/Chaos Dungeon Scripts/Map/MapManager*
  - 스테이지에 진입 시 랜덤으로 맵을 생성해주는 스크립트입니다.
  - 이차원 배열을 이용해 맵 틀을 초기화하고 랜덤한 좌표부터 맵을 생성해 나가는 방식입니다.
<br>
 
```MapManager.cs
public class MapManager : Singleton<MapManager>
{
    [Header("맵 프리팹 변수")]
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Map startMap;
    [SerializeField] private BossMap lastMap;                       //보스맵 프리팹
    [SerializeField] private List<Map> mapList = new List<Map>();   //맵 프리팹을 넣을 리스트
    [SerializeField] private TransMap transMap;                     //맵을 담을 위치 프리팹

    private BossMap bossMap;

    public TransMap[,] maps;                //맵 프리팹이 들어갈 배열
    private Queue<Map> queueMap = new Queue<Map>();

    [Header("맵 생성 관련 커스텀 설정 변수")]
    [SerializeField] private int mapDistance;     //맵 사이의 거리
    [SerializeField] private int maxMapCount;     //스테이지의 총 맵 개수
    private int curMapCount = 0;                  //현재까지 생성한 맵의 카운트

    Map checkLastMap = null; // 마지막 맵을 체크함
    //2차원 배열의 [x, y]값
    public int x, y;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetStartMap();
    }

    //처음 시작할 맵 타입과 위치를 정해주기
    private void SetStartMap()
    {
        maps = new TransMap[x, y];

        //시작 맵은 고정
        Map map = Instantiate(startMap);
        map.type = MapType.Start;
        bossMap = Instantiate(lastMap);
        bossMap.type = MapType.Boss;
        bossMap.transform.position = new Vector3(-50, 50, 0);

        //전체 맵의 가로 혹은 세로가 3칸 이상일 경우 스타트맵은 안쪽 공간에서 시작하기
        int randRow = x > 2 ? Random.Range(1, maps.GetLength(0) - 1) : Random.Range(0, maps.GetLength(0));
        int randCol = y > 2 ? Random.Range(1, maps.GetLength(1) - 1) : Random.Range(0, maps.GetLength(1));

        CreateTransMap(map, randRow, randCol);  //맵을 담을 TransMap 생성
        StructMap(map, randRow, randCol);       //TransMap에 실질적인 맵을 생성하고 다음 후보 타일 선별
        checkLastMap.isLastMap = true;
        map.NextWave();

        GameManager.Map = map;
        GameManager.GetPlayer().transform.position = map.transform.position + map.transform.Find("Ground").GetComponent<TilemapCollider2D>().bounds.center;
        GameManager.Instance.MapCamera();
    }

    //맵을 생성할 때 먼저 트랜스맵을 생성
    private void CreateTransMap(Map map, int row, int col)
    {
        TransMap tmap = Instantiate(transMap, transform);
        maps[row, col] = tmap;
        tmap.Set(map, row, col);
        tmap.transform.position = new Vector3(col * mapDistance, -row * mapDistance);

        SetMapInfo(tmap, map);
    }

    //TransMap과 Map의 정보를 넣어주기
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
        //map이 이동할 수 있는 방향 (이 리스트에 있는 포탈중 랜덤값을 줘 1 ~ list.count만큼 길 생성)
        List<GameObject> mapPortalList = new List<GameObject>();

        CheckDirection(map, row, col, mapPortalList);

        if (mapPortalList.Count > 0)
        {
            //맵 포탈을 몇 개 열 것인지
            int randWayCount = Random.Range(1, mapPortalList.Count + 1);

            for (int i = 0; i < randWayCount; i++)
            {
                curMapCount++;
                if (curMapCount > maxMapCount)
                {
                    OffBool(map, mapPortalList);
                    break;
                }

                //어떤 포탈을 열 것인지
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
            //마지막 맵이 아닌 중립지역이 보스방이 되는거 방지
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

    //맵의 특정 방향이 열려있으면 -> 이 맵의 위치가 특정 방향의 끝이 아니면 -> 리스트에 추가
    
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

        #region 각 방향 조건 체크
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


    //맞붙은 포탈끼리 연결해줌
    private void ConnectPortal(GameObject firstPortal, GameObject secondPortal)
    {
        if (!firstPortal.GetComponent<Portal>() && !secondPortal.GetComponent<Portal>())
            return;

        firstPortal.GetComponent<Portal>().nextPortal = secondPortal.GetComponent<Portal>().transform;
        secondPortal.GetComponent<Portal>().nextPortal = firstPortal.GetComponent<Portal>().transform;
    }

    //포탈 생성 조건에 만족해 리스트에 넣었지만 쓰이지 않은 방향을 false로 만듬
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
```




















