using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eenermy : MonoBehaviour
{
    GameObject player;
    public float attackRange;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            currState = EnemyState.Attack;
        }
    }
}
