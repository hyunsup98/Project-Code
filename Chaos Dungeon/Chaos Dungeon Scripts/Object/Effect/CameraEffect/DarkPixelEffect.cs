using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DarkPixelEffect : MonoBehaviour
{
    [SerializeField] Transform effects;
    public Sprite sprite;
    public SpriteRenderer rander;
    public SpriteRenderer parent;

    public float time = 0.05f;
    public int max = 20;
    float delay = 0;

    Vector2 size;

    void Start()
    {
        rander.sprite = sprite;
        parent.sprite = sprite;
        size = sprite.textureRect.size/10;
    }

    // Update is called once per frame
    void Update()
    {
        if(time < delay)
        {
            if (max > 0)
            {
                max--;
                Transform efft = Instantiate(effects, parent.transform);
                efft.gameObject.SetActive(true);
                if (max == 0)
                {
                    efft.transform.localPosition = new Vector3(0, 0, 0);
                    efft.localScale = new Vector3(size.x, size.y, 3);
                }
                else
                {
                    float x = size.x / 32;
                    float y = size.y / 32;
                    efft.transform.localPosition = new Vector3(Random.Range(-x, x), Random.Range(-y, y), 0);
                    efft.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
                    efft.localScale = new Vector3(Random.Range(size.x*0.05f, size.x * 0.5f), Random.Range(size.y * 0.05f, size.y * 0.5f), 1);
                }
                delay = 0;
            }
        }
        delay+= GameManager.deltaTime;
        if(max <= 10)
        {
            Color c = parent.color;
            if (c.a >= 0)
            {
                c.a -= GameManager.deltaTime;
                rander.color = c;
                parent.color = c;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
