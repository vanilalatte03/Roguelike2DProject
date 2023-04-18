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
        // Menu ��ư ��Ȱ��ȭ
        menuButton.gameObject.SetActive(false);
        // Continue ��ư�� Quit ��ư Ȱ��ȭ
        continueButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        // ���� �Ͻ�����
        Time.timeScale = 0f;
        // �Ͻ����� �޴� Ȱ��ȭ
        pauseMenu.SetActive(true);
    }

    public void ContinueGame()
    {
        // Continue ��ư�� Quit ��ư ��Ȱ��ȭ
        continueButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        // ���� ���
        Time.timeScale = 1f;
        // �Ͻ����� �޴� ��Ȱ��ȭ
        pauseMenu.SetActive(false);
        // Menu ��ư Ȱ��ȭ
        menuButton.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        // ���� ����
        Application.Quit();
    }
}
