using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> spritesList;

    [SerializeField]
    private float frameTime;

    private Image image;
    private WaitForSeconds wait;
    private int currentFrameIndex = 0;

    private void Awake()
    {
        image = GetComponent<Image>();
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
            image.sprite = spritesList[currentFrameIndex];

            yield return wait;

            currentFrameIndex = (currentFrameIndex + 1) % spritesList.Count;
        }
    }
}
