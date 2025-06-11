using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������������ ����ϴ� ������Ʈ ���⿡ �ְ� ������ ���
public class UtilObject : Singleton<UtilObject>
{
    public HpBar hpbar;
    public TextObject text;
    public SoundObject sound;
    public DarkPixelEffect dark;

    //�ش� ��󿡰� ü�¹ٸ� �޾���
    public static HpBar GetHpBar(Transform loc)
    {
        HpBar hp = Instantiate<HpBar>(Instance.hpbar,loc);
        hp.transform.parent = null;
        hp.transform.localScale = Instance.hpbar.transform.localScale;
        hp.transform.parent = loc;
        hp.transform.localPosition = new Vector2(0, -0.5f);
        return hp;
    }

    //�ش� ��ġ�� �ؽ�Ʈ ��ȯ
    public static TextObject SpawnText(string text,Transform loc,float time)
    {
        TextObject txt = TextManager.Get(Instance.text, loc);
        txt.SetText(text);
        txt.Remove(time);
        txt.transform.localScale = new Vector3(1, 1, 1);
        txt.transform.parent = TextManager.Instance.transform;
        return txt;
    }

    //�ش� ��ġ�� ���� ��ȯ (����� GameManager - SoundList ����)
    public static SoundObject PlaySound(string name, Transform loc, float volume, float pitch)
    {
        SoundObject sound = SoundManager.Get(Instance.sound,loc);
        sound.SetSound(name, volume * GameManager.instance.soundSize, pitch);
        return sound;
    }

    //�ش� ��ġ�� ���� ��ȯ (����� GameManager - SoundList ����)
    public static DarkPixelEffect Effect(Sprite sprite, Transform loc)
    {
        DarkPixelEffect darks = Instantiate(Instance.dark, loc.position, Quaternion.identity);        darks.transform.localScale = loc.transform.localScale;
        darks.transform.parent = null;
        darks.sprite = sprite;
        darks.time = 0.02f;
        return darks;
    }
}
