using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> spritesList;

    [SerializeField]
    private float frameTime;

    [SerializeField]
    private Image bgPanel;

    [SerializeField]
    private Image storyPanel;

    private WaitForSeconds wait;
    private int currentFrameIndex = 0;

    [SerializeField]
    private TextMeshProUGUI anywhereText;

    [SerializeField]
    private AnimationCurve fadeCurve;

    private void Awake()
    {
        wait = new WaitForSeconds(frameTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeAnywhere());
        StartCoroutine(GIF());
    }

    private IEnumerator GIF()
    {
        while (true)
        {
            bgPanel.sprite = spritesList[currentFrameIndex];

            yield return wait;

            currentFrameIndex = (currentFrameIndex + 1) % spritesList.Count;
        }
    }

    public void StartStory()
    {
        storyPanel.gameObject.SetActive(true);
        anywhereText.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        LoadingSceneController.LoadScene("BasementMain");
    }

    private IEnumerator FadeAnywhere()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));

            yield return StartCoroutine(Fade(0, 1));
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float curTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1f)
        {
            curTime += Time.deltaTime;
            percent = curTime / 1f;

            Color color = anywhereText.color;
            color.a = Mathf.Lerp(start, end, fadeCurve.Evaluate(percent));

            anywhereText.color = color;

            yield return null;
        }
    }
}
