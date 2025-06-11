using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map : DelayBehaviour
{
    //�� ����
    public MapType type;

    //�� �ܰ��� ���̺���� �����ϴ� ����Ʈ
    public List<Wave> waves = new List<Wave>();

    public Transform camera_MaxSize;
    public Transform Camera_MinSize;

    public GameObject bossPortal;
    public List<GameObject> ablePortal = new List<GameObject>();
    public TransMap transParent;

    //���⺰ ��Ż, �⺻���� SetActive(false)
    public GameObject upPortal;
    public GameObject downPortal;
    public GameObject leftPortal;
    public GameObject rightPortal;

    //���� �ʿ��� �̵������� ������ true, �Ұ����� false
    public bool isUp;
    public bool isDown;
    public bool isLeft;
    public bool isRight;

    public bool isLastMap = false;

    //���� ���������� �Ѱ��� �����Ҽ� �ִ� ��
    public bool isOnly = false;

    private void Start()
    {
        if (ablePortal.Count > 0)
            ActivePortal();
    }

    //�ش� ���� ���͸� �� ��� �� �� ��Ż�� �������ֱ� ���� �Լ�
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

    //MapManager ��ũ��Ʈ���� ���� üũ
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
