using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        menuButton.onClick.AddListener(ShowPauseMenu);
        continueButton.onClick.AddListener(ContinueGame);
        quitButton.onClick.AddListener(QuitGame);
        pauseMenu.SetActive(false);
        continueButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        // Menu 버튼 비활성화
        menuButton.gameObject.SetActive(false);
        // Continue 버튼과 Quit 버튼 활성화
        continueButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        // 게임 일시정지
        Time.timeScale = 0f;
        // 일시정지 메뉴 활성화
        pauseMenu.SetActive(true);
    }

    public void ContinueGame()
    {
        // Continue 버튼과 Quit 버튼 비활성화
        continueButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        // 게임 계속
        Time.timeScale = 1f;
        // 일시정지 메뉴 비활성화
        pauseMenu.SetActive(false);
        // Menu 버튼 활성화
        menuButton.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        // 게임 종료
        Application.Quit();
    }
}
