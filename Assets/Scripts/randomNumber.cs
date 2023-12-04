using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomNumber : MonoBehaviour
{
    public GameObject back;
    public GameObject[] rnd = new GameObject[7];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; ++i)
        {
            int a = Random.Range(0, 6);
            int b = Random.Range(0, 6);

            GameObject tmp = rnd[a];
            rnd[a] = rnd[b];
            rnd[b] = tmp;
        }

        rnd[0].SetActive(true);
        rnd[0].transform.localPosition = new Vector3(-500, 0);
        rnd[1].SetActive(true);
        rnd[2].SetActive(true);
        rnd[2].transform.localPosition = new Vector3(500, 0);

        back.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
