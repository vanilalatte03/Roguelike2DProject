using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    //ȸ���Ǵ� ���ǵ��̴�.
    [SerializeField]
    private GameObject start;
    public float TurnSpeed;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    //�߻�� �Ѿ� ������Ʈ�̴�.
    public GameObject Bullet;

    public float SpawnInterval = 0.5f;
    private float _spawnTimer;

    void Update()
    {

    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        switch(patternIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
    }

    void Shot()
    {
        //360�� �ݺ�
        for (int i = 0; i < 360; i += 13)
        {
            //�Ѿ� ����
            GameObject temp = Instantiate(Bullet);

            //2�ʸ��� ����
            Destroy(temp, 2f);

            //�Ѿ� ���� ��ġ�� (0,0) ��ǥ�� �Ѵ�.
            temp.transform.position = start.transform.position;

            //Z�� ���� ���ؾ� ȸ���� �̷�����Ƿ�, Z�� i�� �����Ѵ�.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("Shot", 2);
        else
            Invoke("Think", 2);
    }

    void Spin()
    {
        //�⺻ ȸ��
        transform.Rotate(Vector3.forward * (TurnSpeed * 100 * Time.deltaTime));

        //���� ���� ó��
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer < SpawnInterval) return;

        //�ʱ�ȭ
        _spawnTimer = 0f;

        //�Ѿ� ����
        GameObject temp = Instantiate(Bullet);

        //2���� �ڵ� ����
        Destroy(temp, 2f);

        //�Ѿ� ���� ��ġ�� ���� �Ա��� �Ѵ�.
        temp.transform.position = start.transform.position;

        //�Ѿ��� ������ ������Ʈ�� �������� �Ѵ�.
        //->�ش� ������Ʈ�� ������Ʈ�� 360�� ȸ���ϰ� �����Ƿ�, Rotation�� ������ ��.
        temp.transform.rotation = transform.rotation;

        if (transform.localEulerAngles.z >= 350)
        {
            curPatternCount++;

            if (curPatternCount < maxPatternCount[patternIndex])
                Invoke("Spin", 2);
            else
                Invoke("Think", 2);
        }
    }

    void Sniper()
    {
        curPatternCount++;

        if(curPatternCount < maxPatternCount[patternIndex])
            Invoke("Sniper", 2);
        else
            Invoke("Think", 2);
    }

    void Gun()
    {
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("Gun", 2);
        else
            Invoke("Think", 2);
    }

    void Arc()
    {
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
            Invoke("Arc", 2);
        else
            Invoke("Think", 2);
    }
}
