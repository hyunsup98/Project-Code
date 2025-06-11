using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : Singleton<ObjectPool<T>> where T : MonoBehaviour
{
    protected Dictionary<string, Queue<T>> poolDatas = new Dictionary<string, Queue<T>>();

    public T Get(T type, Transform pos)
    {
        T data;
        string name = type.name;

        if(!poolDatas.ContainsKey(name))
        {
            poolDatas.Add(name, new Queue<T>());
        }

        Queue<T> q = poolDatas[name];

        if(q.Count <= 0)
        {
            data = Instantiate(type, pos);
            data.name = name;
        }
        else
        {
            data = q.Dequeue();
            data.gameObject.SetActive(true);
        }

        data.transform.position = pos.position;
        data.transform.SetParent(Instance.transform);
        data.gameObject.SetActive(true);
        return data;
    }

    public void Take(T type)
    {
        string name = type.name;
        if(!poolDatas.ContainsKey(name))
        {
            poolDatas.Add(name, new Queue<T>());
        }

        type.gameObject.SetActive(false);
        poolDatas[name].Enqueue(type);
    }
}
