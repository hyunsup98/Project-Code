using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource BGM;     //�������
    public AudioSource Sound;   //ȿ����

    [SerializeField] private List<AudioClip> audioClipList = new List<AudioClip>();
    [SerializeField] private Dictionary<string, AudioClip> dicSounds = new Dictionary<string, AudioClip>();

    private void Start()
    {
        foreach(var clip in audioClipList)
        {
            dicSounds.Add(clip.name, clip);
        }
    }

    //������� �ٲٱ�
    public void SetBGM(AudioClip bgm)
    {
        BGM.clip = bgm;
        BGM.Play();
    }

    //ȿ���� �ٲٱ�
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
