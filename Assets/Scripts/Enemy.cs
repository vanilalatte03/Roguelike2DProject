using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyController의 Rotation 값이 바뀌면 스프라이트도 같이 돌아가서
// 스프라이트만 가만히 있게 하려고 잠깐 임시로 만든 스크립트

public class Enemy : MonoBehaviour
{
    public GameObject enemyCon;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = enemyCon.transform.position;
    }
}
