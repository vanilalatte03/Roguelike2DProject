using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneController : MonoBehaviour
{
    public static bool isClear;
    private AudioSource source;
            
    [SerializeField]
    private Button button;

    [SerializeField]
    private Sprite clearImg;

    [SerializeField]
    private Sprite overImg;

    [SerializeField]
    private AudioClip clearClip;

    [SerializeField]
    private AudioClip overClip;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (isClear)
        {
            button.image.sprite = clearImg;
            source.clip = clearClip;
            source.volume = 0.15f;
        } 
        
        else
        {
            button.image.sprite = overImg;
            source.clip = overClip;
        }

        source.Play();
    }

    public void ReStart()
    {
        LoadingSceneController.LoadScene("StartScene");
    }
}
