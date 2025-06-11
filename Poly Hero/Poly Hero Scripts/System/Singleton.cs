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
            //�̱����� ����� ������Ʈ�� �ٸ� ������Ʈ�� �ڽ��� ��� root ��ġ�� ������Ʈ�� �������� ����
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            //������ �ֻ��� root�� ��� ������ �������� ����
            DontDestroyOnLoad(gameObject);
        }
    }
}
