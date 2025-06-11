using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] AudioSource bgm;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.Instance == null)
        {
            SceneManager.LoadScene(0);
        }  
    }


    private void Start()
    {
        GameManager.Instance.bgm = bgm;
        bgm.volume = GameManager.instance.bgmSize;
    }

}
