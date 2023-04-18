using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;

    [SerializeField] private GameObject GameOverBack;
    [SerializeField] private Button RestartButton;

    public string retrySceneName = "BasementMain";

    private void Start()
    {
       
        RestartButton.onClick.AddListener(RestartGame);
        instance = this;
        GameOverBack.SetActive(false);
        RestartButton.gameObject.SetActive(false);
    }


    public static void ShowGameOverBack()
    {
        instance.GameOverBack.SetActive(true);
        instance.RestartButton.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }


    public void RestartGame()
    {

        SceneManager.LoadScene(retrySceneName);
        Time.timeScale = 1f;
    }




    void Update()
    {

    }
}
