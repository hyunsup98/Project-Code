using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//여러군데에서 사용하는 오브젝트 여기에 넣고 꺼내서 사용
public class UtilObject : Singleton<UtilObject>
{
    public HpBar hpbar;
    public TextObject text;
    public SoundObject sound;
    public DarkPixelEffect dark;

    //해당 대상에게 체력바를 달아줌
    public static HpBar GetHpBar(Transform loc)
    {
        HpBar hp = Instantiate<HpBar>(Instance.hpbar,loc);
        hp.transform.parent = null;
        hp.transform.localScale = Instance.hpbar.transform.localScale;
        hp.transform.parent = loc;
        hp.transform.localPosition = new Vector2(0, -0.5f);
        return hp;
    }

    //해당 위치에 텍스트 소환
    public static TextObject SpawnText(string text,Transform loc,float time)
    {
        TextObject txt = TextManager.Get(Instance.text, loc);
        txt.SetText(text);
        txt.Remove(time);
        txt.transform.localScale = new Vector3(1, 1, 1);
        txt.transform.parent = TextManager.Instance.transform;
        return txt;
    }

    //해당 위치에 사운드 소환 (사운드는 GameManager - SoundList 참조)
    public static SoundObject PlaySound(string name, Transform loc, float volume, float pitch)
    {
        SoundObject sound = SoundManager.Get(Instance.sound,loc);
        sound.SetSound(name, volume * GameManager.instance.soundSize, pitch);
        return sound;
    }

    //해당 위치에 사운드 소환 (사운드는 GameManager - SoundList 참조)
    public static DarkPixelEffect Effect(Sprite sprite, Transform loc)
    {
        DarkPixelEffect darks = Instantiate(Instance.dark, loc.position, Quaternion.identity);        darks.transform.localScale = loc.transform.localScale;
        darks.transform.parent = null;
        darks.sprite = sprite;
        darks.time = 0.02f;
        return darks;
    }
}
