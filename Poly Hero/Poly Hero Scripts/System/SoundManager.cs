using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource BGM;     //배경음악
    public AudioSource Sound;   //효과음

    [SerializeField] private List<AudioClip> audioClipList = new List<AudioClip>();
    [SerializeField] private Dictionary<string, AudioClip> dicSounds = new Dictionary<string, AudioClip>();

    private void Start()
    {
        foreach(var clip in audioClipList)
        {
            dicSounds.Add(clip.name, clip);
        }
    }

    //배경음악 바꾸기
    public void SetBGM(AudioClip bgm)
    {
        BGM.clip = bgm;
        BGM.Play();
    }

    //효과음 바꾸기
    public void SetSound(AudioClip sound, Transform trans)
    {
        AudioSource.PlayClipAtPoint(sound, trans.position);
        Sound.PlayOneShot(sound);
    }

    public void SetBGMString(string bgmString)
    {
        if (!dicSounds.ContainsKey(bgmString))
            return;

        SetBGM(dicSounds[bgmString]);
    }

    public void SetSoundString(string soundString, Transform trans)
    {
        if (!dicSounds.ContainsKey(soundString))
            return;

        SetSound(dicSounds[soundString], trans);
    }
}
