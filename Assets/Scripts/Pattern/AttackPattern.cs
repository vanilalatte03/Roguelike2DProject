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

    public float SpawnInterval = 0.5f;
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
        Invoke("Attack", 5f);
    }

    void Attack()
    {
        anim.SetTrigger("ChangeAnimation");

        switch (patternIndex)
        {
            case 0:
                Invoke("Shot", 2f);
                break;
            case 1:
                Invoke("Spin", 2f);
                break;
            case 2:
                Invoke("Follow", 2f);
                break;
            case 3:
                Invoke("Gun", 2f);
                break;
            case 4:
                Invoke("Arc", 2f);
                break;
        }

        patternIndex = patternIndex >= 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;
    }

    void Shot()
    {
        for (int i = 0; i < 360; i += 13)
        {
            //�Ѿ� ����
            GameObject temp = Instantiate(Bullet);

            //2�ʸ��� ����
            Destroy(temp, 5f);

            //�Ѿ� ���� ��ġ�� (0,0) ��ǥ�� �Ѵ�.
            temp.transform.position = start.transform.position;

            //Z�� ���� ���ؾ� ȸ���� �̷�����Ƿ�, Z�� i�� �����Ѵ�.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
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
            Invoke("Spin", 0f);
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
        temp.transform.position = start.transform.position;

        //�Ѿ��� ������ ������Ʈ�� �������� �Ѵ�.
        //->�ش� ������Ʈ�� ������Ʈ�� 360�� ȸ���ϰ� �����Ƿ�, Rotation�� ������ ��.
        temp.transform.rotation = transform.rotation;
    }

    void Follow()
    {
        if (player.position.x - transform.position.x < 0)
        {
            attackTransform.localPosition = new Vector3(-0.25f, -0.2f, 0f);
        }

        else
        {
            attackTransform.localPosition = new Vector3(0.25f, -0.2f, 0f);
        }

        GameObject prefab = Instantiate(Bullet, attackTransform.position, Quaternion.identity);
        Bullet bullet = prefab.GetComponent<Bullet>();
        bullet.GetPlayer(player.transform);

        GameObject temp = Instantiate(Bullet);

        Destroy(temp, 5f);

        temp.transform.position = start.transform.position;

        Vector2 dirVec = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
        Quaternion rotTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        temp.transform.rotation = Quaternion.Euler(dirVec.normalized);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            anim.SetTrigger("ChangeAnimation");

            Invoke("Follow", 2f);
        }
        else
        {
            Invoke("Attack", 2f);
        }
    }

    void Gun()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject temp = Instantiate(Bullet);

            Destroy(temp, 5f);

            temp.transform.position = start.transform.position;

            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;
            temp.transform.rotation = Quaternion.Euler(dirVec.normalized);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
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
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            anim.SetTrigger("ChangeAnimation");

            Invoke("Arc", 2f);
        }
        else
        {
            Invoke("Attack", 2f);
        }
    }
}
