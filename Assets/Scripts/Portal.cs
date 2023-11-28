using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public enum stageType
    {
        Stage1,
        Stage2
    };

    public stageType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (type)
            {
                case stageType.Stage1:
                    SceneManager.LoadScene("BasementBoss");
                    break;
                case stageType.Stage2:
                    SceneManager.LoadScene("StoneBoss");
                    break;
            }
        }
    }
}
