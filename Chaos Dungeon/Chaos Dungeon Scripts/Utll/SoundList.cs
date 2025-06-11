using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ����Ʈ
//Inspector���� list�� ���带 ������ ��ųʸ��� ��ȯ�� ����

public class SoundList : Singleton<SoundList>
{
    [SerializeField] List<AudioClip> sounds;
    Dictionary<string, AudioClip> sound = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        foreach(AudioClip s in sounds){
            sound.Add(s.name, s);
        }
    }

    public static AudioClip GetSound(string name)
    {
        if (!Instance.sound.ContainsKey(name)) return null;
        return Instance.sound[name];
    }
}
