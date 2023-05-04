using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random : MonoBehaviour
{
    public GameObject[] rnd = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 10; ++i)
        {
            int a = Random.Range(0, 4);
            int b = Random.Range(0, 4);

            GameObject tmp = rnd[a];
            rnd[a] = rnd[b];
            rnd[b] = tmp;
        }

        rnd[0].SetActive(true);
        rnd[0].transform.localPosition = new Vector3(-500, 0);
        rnd[1].SetActive(true);
        rnd[2].SetActive(true);
        rnd[2].transform.localPosition = new Vector3(500, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
