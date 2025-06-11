using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

//�Ȱ��� ������Ʈ �ʱⰪ�� �ٲ㼭 ����Ҷ�
//Get(Ÿ��,��ġ) �� ������Ʈ�� ����ų� ������� ������Ʈ�� ������
//Remove(������Ʈ) �ش� ������Ʈ�� ����ķ� ����
//T�� ������ƮǮ�� ���� �ڷ��� ex) EntityManager<Entity>

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


