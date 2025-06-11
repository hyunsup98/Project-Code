using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapType
{
    Start,
    No,
    Monster,
    Heal,
    Boss,
    Box
}

[System.Serializable]
public class MapIcon
{
    public MapType type;
    public Sprite icon;
}

public class MapView : MonoBehaviour
{
    //아이콘 베이스
    [SerializeField] Image img;
    //플레이어 현재 위치 표시
    [SerializeField] Image playerloc;
    //아이콘 소환 위치
    [SerializeField] RectTransform imglocal;
    //아이콘 종류 세팅
    [SerializeField] List<MapIcon> sprites;

    //길 베이스
    [SerializeField] Image road_img;
    [SerializeField] Transform road_local;

    Image[,] imgs;

    bool isCraete = false;

    public void ReCreate()
    {
        List<GameObject> remove = new List<GameObject>();
        for(int i = 3; i < imglocal.childCount; i++)
        {
            GameObject obj = imglocal.transform.GetChild(i).gameObject;
            remove.Add(obj);
            Destroy(obj);
        }
        for (int i = 1; i < road_local.childCount; i++)
        {
            GameObject obj = road_local.transform.GetChild(i).gameObject;
            remove.Add(obj);
            Destroy(obj);
        }
        isCraete = false;
    }

    private void OnEnable()
    {
        if(!isCraete) Start();
        int x = MapManager.Instance.x;
        int y = MapManager.Instance.y;
        imglocal.position = new Vector3(-200,1700);
        for (int j = 0; j < x; j++)
        {
            for (int i = 0; i < y; i++)
            {
                TransMap map = MapManager.Instance.maps[i, j];
                if (map == null) continue;
                
                Image image = imgs[i, j];
                if (map.map.waves != null && map.map.waves.Count > 0 && GameManager.Map != map.map)
                    image.sprite = GetIcon(MapType.No);
                else
                {
                    image.sprite = GetIcon(map.map.type);
                    Image ig;
                    if (map.map.isLeft)
                    {
                        ig = CreateRoad(j, i);
                        ig.rectTransform.localPosition += new Vector3(-100, 0);
                        ig.rectTransform.sizeDelta = new Vector3(200, 20);
                        imgs[i, j - 1].gameObject.SetActive(true);
                    }
                    if (map.map.isRight)
                    {
                        ig = CreateRoad(j, i);
                        ig.rectTransform.localPosition += new Vector3(100, 0);
                        ig.rectTransform.sizeDelta = new Vector3(200, 20);
                        imgs[i, j + 1].gameObject.SetActive(true);
                    }
                    if (map.map.isUp)
                    {
                        ig = CreateRoad(j, i);
                        ig.rectTransform.localPosition += new Vector3(0, 100);
                        ig.rectTransform.sizeDelta = new Vector3(20, 200);
                        imgs[i - 1, j].gameObject.SetActive(true);
                    }
                    if (map.map.isDown)
                    {
                        ig = CreateRoad(j, i);
                        ig.rectTransform.localPosition += new Vector3(0, -100);
                        ig.rectTransform.sizeDelta = new Vector3(20, 200);
                        imgs[i + 1, j].gameObject.SetActive(true);
                    }
                }
                if (GameManager.Map == map.map)
                {
                    playerloc.rectTransform.localPosition = image.rectTransform.localPosition;
                }
            }
        }
    }

    void Start()
    {
        isCraete = true;
        int x = MapManager.Instance.x;
        int y = MapManager.Instance.y;
        if(imgs == null)
        {
            imgs = new Image[x, y];
        }
        for(int j = 0; j< x; j++)
        {
            for(int i = 0; i < y; i++)
            {
                TransMap map = MapManager.Instance.maps[i,j];
                if(map != null)
                {
                    Image image;
                    if (imgs[i, j] == null)
                    {
                        image = Instantiate(img, imglocal);
                        image.rectTransform.localPosition = new Vector3(j * 300, i * -300);
                        imgs[i, j] = image;
                        imgs[i, j].gameObject.SetActive(false);
                    }

                    image = imgs[i, j];

                    if(GameManager.Map == map.map)
                    {
                        imgs[i, j].gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    Image CreateRoad(float x,float y)
    {
        Image image = Instantiate(road_img, imglocal);
        image.gameObject.SetActive(true);
        image.rectTransform.localPosition = new Vector3(x * 300, y * -300);
        image.transform.SetParent(road_local);
        return image;
    }
    Sprite GetIcon(MapType type)
    {
        foreach (var icon in sprites)
        {
            if (icon.type == type)
            {
                return icon.icon;
            }
        }
        return null;
    }
    float size = 1;

    void Update()
    {
        float sz = size;
        sz = Mathf.Clamp(size%3, 1f, 3f);
        playerloc.transform.localScale = new Vector3 (sz, sz, sz);
        size += Time.deltaTime * 2;
    }
}
