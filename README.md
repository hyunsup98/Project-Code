# 포트폴리오 코드를 정리해놓은 Repository 입니다.


# 0. 카오스 던전

- **프로젝트 소개**
  - 2D 횡스크롤뷰 로그라이크 게임입니다.
    
    총 3개의 스테이지로 이루어져있습니다.
  
    패시브 스킬을 정하고 여러 무기들을 활용하여 몬스터를 물리치고 마지막 보스를 잡아보세요.


## 0-1 주요 기능 코드

- 오브젝트 풀
  - 오브젝트의 효율적인 관리를 위해 오브젝트 풀링 기법을 사용하였습니다.
  - 오브젝트 풀링을 사용하는 객체가 여러개이므로 제네릭을 활용해 커스텀 ObjectPool 스크립트를 작성했습니다.
 
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

- 동적 맵 생성 알고리즘
  - 각 스테이지에 진입할 때 초기 맵 세팅 내에서 랜덤으로 맵을 생성해주는 맵 스크립트입니다.




















