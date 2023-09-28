using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField]
    private AudioSource[] bgmsSources;

    [SerializeField]
    private AudioSource[] effectSources;

    [SerializeField]
    private string[] playSoundNames;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        playSoundNames = new string[effectSources.Length];
    }

    // Sound �޴� ��ư Ŭ��
    public void ToggleBGM()
    {
        bgmsSources[0].enabled = !bgmsSources[0].enabled;
    }

    // �ܺο��� ȣ���ϴ� ����Ʈ ����
    public void PlaySoundEffect(string _name)
    {
        // �ݺ����� ���� ��ġ�ϴ� ����Ʈ �̸��� ã�´�.
        for (int i = 0; i < effectSounds.Length; i++)
        {
            // ��ġ�ϴ� ���� ã������?
            if (_name == effectSounds[i].name)
            {
                // ��� ������ ���� ����� �ҽ��� ������ ���
                for (int j = 0; j < effectSources.Length; j++) 
                {
                    if (!effectSources[j].isPlaying)
                    {
                        playSoundNames[j] = effectSounds[i].name;
                        // Ŭ���� ��ü �ϰ� ���
                        effectSources[j].clip = effectSounds[i].clip;
                        effectSources[j].Play();

                        // ����� �Ϸ������� �Լ��� ����
                        return;
                    }
                }

                Debug.Log("��� ����� �ҽ��� ��� ���̴�. �߰��� ����");
                return;
            }   
        }

        Debug.Log(_name + "�� ��ϵ� ���尡 �ƴ�");
        return;
    }

    // ��� ���� ����Ʈ�� �����ϴ� �Լ�
    public void StopAllSoundEffect()
    {
        for (int i = 0; i < effectSources.Length; i++)
        {
            effectSources[i].Stop();
        }
    }
}
