using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EnemyController�� Rotation ���� �ٲ�� ��������Ʈ�� ���� ���ư���
// ��������Ʈ�� ������ �ְ� �Ϸ��� ��� �ӽ÷� ���� ��ũ��Ʈ

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
