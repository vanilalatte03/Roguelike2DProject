using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        wait = new WaitForSeconds(frameTime);
    }

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void StartGame()
    {   
        LoadingSceneController.LoadScene("BasementMain");
    }
}
