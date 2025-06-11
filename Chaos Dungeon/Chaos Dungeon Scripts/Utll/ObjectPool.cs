using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

//똑같은 오브젝트 초기값만 바꿔서 사용할때
//Get(타입,위치) 새 오브젝트를 만들거나 사용후인 오브젝트를 가져옴
//Remove(오브젝트) 해당 오브젝트를 사용후로 선언
//T는 오브젝트풀로 사용될 자료형 ex) EntityManager<Entity>

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


