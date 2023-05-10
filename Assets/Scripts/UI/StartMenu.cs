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

    private void Awake()
    {
        instance = this;
        StartButton = GetComponent<Button>();
    }

    private void Start()
    {

        StartButton.onClick.AddListener(StartGame);
        StartButton.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(StartSceneName);
    }
}