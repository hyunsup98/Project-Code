using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
        set { }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else if (Instance != this)
            Destroy(gameObject);

        if(transform.parent != null && transform.root != null)
        {
            //싱글톤을 사용한 오브젝트가 다른 오브젝트의 자식일 경우 root 위치의 오브젝트를 삭제하지 않음
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            //본인이 최상위 root인 경우 본인을 삭제하지 않음
            DontDestroyOnLoad(gameObject);
        }
    }
}
