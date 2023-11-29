using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public enum stageType
    {
        toBoss1,
        toStage2,
        toBoss2,
        toClear
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
                case stageType.toBoss1:
                    SceneManager.LoadScene("BasementBoss");
                    break;
                case stageType.toStage2:
                    LoadingSceneController.LoadScene("StoneMain");
                    break;
                case stageType.toBoss2:
                    SceneManager.LoadScene("StoneBoss");
                    break;
                case stageType.toClear:
                    EndSceneController.isClear = true;
                    SceneManager.LoadScene("EndingScene");
                    break;
            }
        }
    }
}
