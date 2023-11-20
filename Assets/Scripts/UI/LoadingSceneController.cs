using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private Image loadingImg;

    [SerializeField]
    private List<Sprite> loadingSpritesList;

    [SerializeField]
    private float frameTime;

    private int currentFrameIndex = 0;
    private WaitForSeconds wait;

    private void Awake()
    {
        wait = new WaitForSeconds(frameTime);
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
        StartCoroutine(GIF());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");

    }

    private IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        op.allowSceneActivation = false;

        float timer = 0f;

        // 씬이 끝날때 까지 반복 
        while(!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            } 
            
            // 페이크 로딩
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                } 
            }
        }
    }

    private IEnumerator GIF()
    {
        while (true)
        {
            loadingImg.sprite = loadingSpritesList[currentFrameIndex];

            yield return wait;

            currentFrameIndex = (currentFrameIndex + 1) % loadingSpritesList.Count;
        }
    }
}
