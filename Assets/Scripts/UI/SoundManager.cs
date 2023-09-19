using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource bgm;

    private void Awake()
    {
        bgm = GetComponent<AudioSource>();
    }

    // Sound �޴� ��ư Ŭ��
    public void ToggleBGM()
    {
        bgm.enabled = !bgm.enabled;
    }
}
