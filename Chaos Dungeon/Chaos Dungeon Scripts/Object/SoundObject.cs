using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ش� ��ġ�� �Ҹ��� ����ϴ� ��ü
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

    //��� ����� ������Ʈ Ǯ�� �ǵ���
    void Update()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            SoundManager.Remove(this);
        }
    }
}
