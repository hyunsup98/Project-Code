using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

//�Ѿ� ,���� �� ��ӹ���,Inspector���� ���� �ٲ� �������� Ŭ������ ����Ҷ�
//���� �̸����� �ٸ� Queue�� ����Ǵ� ����� ������Ʈ�� �Բ� �������� <������ ��ӹ��� ������Ʈ>
//Get(Ÿ��,��ġ) �� ������Ʈ�� ����ų� ������� ������Ʈ�� ������
//Remove(������Ʈ) �ش� ������Ʈ�� ����ķ� ����
//T�� ������ƮǮ�� ���� ����(���) �ڷ��� ex) EntityManager<Entity>

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
