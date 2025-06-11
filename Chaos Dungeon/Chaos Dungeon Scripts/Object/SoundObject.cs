using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치에 소리를 재생하는 객체
public class SoundObject : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void SetSound(string name, float volume, float pitch)
    {
        audioSource.clip = SoundList.GetSound(name);
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
    }

    //재생 종료시 오브젝트 풀로 되돌림
    void Update()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            SoundManager.Remove(this);
        }
    }
}
