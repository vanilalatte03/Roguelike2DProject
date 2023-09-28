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

    // Sound 메뉴 버튼 클릭
    public void ToggleBGM()
    {
        bgmsSources[0].enabled = !bgmsSources[0].enabled;
    }

    // 외부에서 호출하는 이펙트 사운드
    public void PlaySoundEffect(string _name)
    {
        // 반복문을 돌려 일치하는 이펙트 이름을 찾는다.
        for (int i = 0; i < effectSounds.Length; i++)
        {
            // 일치하는 것을 찾았으면?
            if (_name == effectSounds[i].name)
            {
                // 재생 중이지 않은 오디오 소스를 선택해 재생
                for (int j = 0; j < effectSources.Length; j++) 
                {
                    if (!effectSources[j].isPlaying)
                    {
                        playSoundNames[j] = effectSounds[i].name;
                        // 클립을 교체 하고 재생
                        effectSources[j].clip = effectSounds[i].clip;
                        effectSources[j].Play();

                        // 재생을 완료했으니 함수를 종료
                        return;
                    }
                }

                Debug.Log("모든 오디오 소스가 사용 중이다. 추가를 권장");
                return;
            }   
        }

        Debug.Log(_name + "는 등록된 사운드가 아님");
        return;
    }

    // 모든 사운드 이팩트를 정지하는 함수
    public void StopAllSoundEffect()
    {
        for (int i = 0; i < effectSources.Length; i++)
        {
            effectSources[i].Stop();
        }
    }
}
