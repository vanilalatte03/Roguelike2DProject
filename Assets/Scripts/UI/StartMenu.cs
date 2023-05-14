using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public static StartMenu instance;

    [SerializeField] private Button StartButton;

    public string StartSceneName = "BasementMain";


    public void OnCilckNewGame()
    {
        SceneManager.LoadScene(StartSceneName);
        
    }

    public void Awake()
    {
        instance = this;
        StartButton = GetComponent<Button>();
    }

    public void Start()
    {

        StartButton.onClick.AddListener(StartGame);
        StartButton.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(StartSceneName);
    }
}