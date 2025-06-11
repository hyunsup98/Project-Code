using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

//총알 ,몬스터 등 상속받은,Inspector에서 값을 바꾼 여러가지 클래스를 사용할때
//같은 이름별로 다른 Queue에 저장되니 비슷한 오브젝트를 함께 관리가능 <같은걸 상속받은 오브젝트>
//Get(타입,위치) 새 오브젝트를 만들거나 사용후인 오브젝트를 가져옴
//Remove(오브젝트) 해당 오브젝트를 사용후로 선언
//T는 오브젝트풀로 사용될 상위(상속) 자료형 ex) EntityManager<Entity>

public class ObjectPoolGroup<T> : Singleton<ObjectPoolGroup<T>> where T: MonoBehaviour
{
    protected Dictionary<string, Queue<T>> objects = new Dictionary<string, Queue<T>>();

    public static Dictionary<string, Queue<T>> GetDictionary()
    {
        return Instance.objects;
    }

    public static T Get(T type, Transform pos)
    {
        string name = type.name;

        if (!Instance.objects.ContainsKey(name))
        {
            Instance.objects.Add(name, new Queue<T>());
        }
        Queue<T> objq = Instance.objects[name];

        if (objq.Count <= 0)
        {
            T o = Instantiate(type, pos);
            o.name = name;
            objq.Enqueue(o);
        }
        type = objq.Dequeue();
        type.transform.position = pos.position;
        type.gameObject.SetActive(true);
        type.transform.parent = Instance.transform;
        return type;
    }

    public static void Remove(T obj)
    {
        if (obj == null) return;
        string name = obj.name;
        if (!Instance.objects.ContainsKey(name))
        {
            Instance.objects.Add(name, new Queue<T>());
        }

        obj.gameObject.SetActive(false);
        Instance.objects[name].Enqueue(obj);
    }
}
