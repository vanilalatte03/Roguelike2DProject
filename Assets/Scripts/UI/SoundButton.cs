using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public AudioSource AudioSource; 

    private Button SoundAdButton; 

    private void Start()
    {
        SoundAdButton = GetComponent<Button>();
        SoundAdButton.onClick.AddListener(ToggleMusic);
    }

    private void ToggleMusic()
    {
        if (AudioSource.enabled)
        {
            AudioSource.enabled = false; 
        }
        else
        {
            AudioSource.enabled = true; 
        }
    }
}
