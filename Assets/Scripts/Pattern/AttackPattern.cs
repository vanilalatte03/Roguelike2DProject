using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    private int spinCnt;
    private int followCnt;

    private Transform player;
    [SerializeField]
    Transform attackTransform;
    [SerializeField]
    private GameObject start;
    public float TurnAngle;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    //�߻�� �Ѿ� ������Ʈ�̴�.
    public GameObject Bullet;

    public float SpawnInterval;
    private float _spawnTimer;

    void Awake()
    {
        player = GameController.instance.player.transform;
        spinCnt = 0;
        Invoke("Starting", 2f); // ���� ���� ���� ���� ��ٸ��� ���� ����
    }

    void Starting()
    {
        anim.SetTrigger("ChangeAnimation");
        Invoke("Attack", 3f);
    }

    void Attack()
    {
        SoundManager.instance.PlaySoundEffect("����ũ������");
        anim.SetTrigger("ChangeAnimation");
        
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                Invoke("Shot", 2f);
                break;
            case 1:
                Invoke("Spin", 2f);
                break;
            case 2:
                Invoke("Gun", 2f);
                break;
            case 3:
                Invoke("Arc", 2f);
                break;
        }

        patternIndex = patternIndex >= 3 ? 0 : patternIndex + 1;
    }

    void Shot()
    {
        int rnd = Random.Range(9, 14);
        for (int i = 0; i < 360; i += rnd)
        {
            //�Ѿ� ����
            GameObject temp = Instantiate(Bullet);

            //2�ʸ��� ����
            Destroy(temp, 5f);

            //�Ѿ� ���� ��ġ�� (0,0) ��ǥ�� �Ѵ�.
            temp.transform.position = start.transform.position + new Vector3(0.2f, 0, 0);

            //Z�� ���� ���ؾ� ȸ���� �̷�����Ƿ�, Z�� i�� �����Ѵ�.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            SoundManager.instance.PlaySoundEffect("����ũ������");
            anim.SetTrigger("ChangeAnimation");

            Invoke("Shot", 2f);
        }
        else
        {
            Invoke("Attack", 2f);
        }
    }

    void Spin()
    {
        spinCnt++;

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Spin();
        }
        else
        {
            Invoke("Attack", 2f);
        }
    }

    void Update()
    {
        if (spinCnt > 0)
        {
            SpinUpdate();
        }
    }

    void SpinUpdate()
    {
        if (transform.localEulerAngles.z == 345)
        {
            SpawnInterval = Random.Range(7, 12);
            transform.Rotate(Vector3.forward * TurnAngle);
            spinCnt--;
            return;
        }

        //���� ���� ó��
        _spawnTimer += 0.1f;
        if (_spawnTimer < SpawnInterval) return;
        transform.Rotate(Vector3.forward * TurnAngle);

        //�ʱ�ȭ
        _spawnTimer = 0f;

        //�Ѿ� ����
        GameObject temp = Instantiate(Bullet);

        //2���� �ڵ� ����
        Destroy(temp, 5f);

        //�Ѿ� ���� ��ġ�� ���� �Ա��� �Ѵ�.
        temp.transform.position = start.transform.position + new Vector3(0.2f, 0, 0);

        //�Ѿ��� ������ ������Ʈ�� �������� �Ѵ�.
        //->�ش� ������Ʈ�� ������Ʈ�� 360�� ȸ���ϰ� �����Ƿ�, Rotation�� ������ ��.
        temp.transform.rotation = transform.rotation;
    }

    void Gun()
    {
        float rnd = Random.Range(0f, 360f);

        for (int i = 0; i < 5; i++)
        {
            //�Ѿ� ����
            GameObject temp = Instantiate(Bullet);

            //2�ʸ��� ����
            Destroy(temp, 5f);

            //�Ѿ� ���� ��ġ�� (0,0) ��ǥ�� �Ѵ�.
            temp.transform.position = start.transform.position + new Vector3(0.2f, 0, 0);

            //Z�� ���� ���ؾ� ȸ���� �̷�����Ƿ�, Z�� i�� �����Ѵ�.
            temp.transform.rotation = Quaternion.Euler(0, 0, rnd - (i * 10));
        }


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            SoundManager.instance.PlaySoundEffect("����ũ������");
            anim.SetTrigger("ChangeAnimation");

            Invoke("Gun", 2f);
        }
        else
        {
            Invoke("Attack", 2f);
        }
    }

    void Arc()
    {
        float rnd = Random.Range(0f, 360f);

        //�Ѿ� ����
        GameObject temp = Instantiate(Bullet);

        //2�ʸ��� ����
        Destroy(temp, 5f);

        //�Ѿ� ���� ��ġ�� (0,0) ��ǥ�� �Ѵ�.
        temp.transform.position = start.transform.position + new Vector3(0.2f, 0, 0);

        //Z�� ���� ���ؾ� ȸ���� �̷�����Ƿ�, Z�� i�� �����Ѵ�.
        temp.transform.rotation = Quaternion.Euler(0, 0, rnd);

        curPatternCount++;

        if (curPatternCount < 100)
        {
            Debug.Log(maxPatternCount[patternIndex]);
            Invoke("Arc", 0.1f);
        }
        else
        {
            Invoke("Attack", 2f);
        }
    }
}
