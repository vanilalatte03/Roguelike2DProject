using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI storyText;

    [SerializeField]
    private string story;
    
    public void OnEnable()
    {
        StartCoroutine(Typing());
    }

    private IEnumerator Typing()
    {
        for (int i = 0; i <= story.Length; i++)
        {
            storyText.text = story.Substring(0, i);

            yield return new WaitForSeconds(0.15f);
        }
    }
}
