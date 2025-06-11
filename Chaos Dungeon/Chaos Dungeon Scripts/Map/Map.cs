using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map : DelayBehaviour
{
    //맵 종류
    public MapType type;

    //각 단계의 웨이브들을 관리하는 리스트
    public List<Wave> waves = new List<Wave>();

    public Transform camera_MaxSize;
    public Transform Camera_MinSize;

    public GameObject bossPortal;
    public List<GameObject> ablePortal = new List<GameObject>();
    public TransMap transParent;

    //방향별 포탈, 기본값은 SetActive(false)
    public GameObject upPortal;
    public GameObject downPortal;
    public GameObject leftPortal;
    public GameObject rightPortal;

    //현재 맵에서 이동가능한 방향은 true, 불가능은 false
    public bool isUp;
    public bool isDown;
    public bool isLeft;
    public bool isRight;

    public bool isLastMap = false;

    //오직 스테이지당 한개만 존재할수 있는 맵
    public bool isOnly = false;

    private void Start()
    {
        if (ablePortal.Count > 0)
            ActivePortal();
    }

    //해당 맵의 몬스터를 다 잡고 난 뒤 포탈을 생성해주기 위한 함수
    public void OpenPortal()
    {
        foreach(var portal in ablePortal)
        {
            portal.GetComponent<Portal>().IsOpen(true);
            if(isLastMap)
                bossPortal.SetActive(true);
        }
    }

    public void ActivePortal()
    {
        for(int i = 0; i < ablePortal.Count; i++)
        {
            ablePortal[i].SetActive(true);
        }
    }

    //MapManager 스크립트에서 조건 체크
    public Map CheckBool(bool isDir)
    {
        Map map = isDir == true ? this : null;
        return map;
    }

    public void NextWave()
    {
        if (MonsterManager.GetSpawnCount() == 0)
        {
            if (waves.Count != 0)
            {
                Delay(() =>
                {
                    if (waves.Count > 0)
                    {
                        waves[0].map = this;
                        waves[0].gameObject.SetActive(true);
                        waves.RemoveAt(0);
                    }
                }, 1);
            }
            else
                OpenPortal();
        }
    }
}
