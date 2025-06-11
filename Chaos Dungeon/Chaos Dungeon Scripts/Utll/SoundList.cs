using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사운드 리스트
//Inspector에서 list에 사운드를 넣으면 딕셔너리로 변환후 저장

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
