using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    private int spinCnt;
    private int followCnt;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject start;
    public float TurnAngle;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    //발사될 총알 오브젝트이다.
    public GameObject Bullet;

    public float SpawnInterval = 0.5f;
    private float _spawnTimer;

    void Awake()
    {
        spinCnt = 0;
        anim.SetBool("Think", true);
        Invoke("Think", 1f);
    }

    void Think()
    {
        anim.SetBool("Think", false);

        switch (patternIndex)
        {
            case 0:
                Shot();
                break;
            case 1:
                Spin();
                break;
            case 2:
                Follow();
                break;
            case 3:
                Gun();
                break;
            case 4:
                Arc();
                break;
        }

        patternIndex = patternIndex >= 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;
    }

    void Shot()
    {
        //360번 반복
        for (int i = 0; i < 360; i += 13)
        {
            //총알 생성
            GameObject temp = Instantiate(Bullet);

            //2초마다 삭제
            Destroy(temp, 5f);

            //총알 생성 위치를 (0,0) 좌표로 한다.
            temp.transform.position = start.transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            anim.SetBool("Think", true);
            Invoke("Shot", 2f);
        }
        else
        {
            Invoke("Think", 2f);
        }
    }

    void Spin()
    {
        spinCnt++;

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            anim.SetBool("Think", true);
            Invoke("Spin", 2f);
        }
        else
        {
            Invoke("Think", 2f);
        }
    }

    void Update()
    {
        if (spinCnt > 0)
        {
            if (transform.localEulerAngles.z == 345)
            {
                transform.Rotate(Vector3.forward * TurnAngle);
                spinCnt--;
                return;
            }

            //생성 간격 처리
            _spawnTimer += 0.1f;
            if (_spawnTimer < SpawnInterval) return;
            transform.Rotate(Vector3.forward * TurnAngle);

            //초기화
            _spawnTimer = 0f;

            //총알 생성
            GameObject temp = Instantiate(Bullet);

            //2초후 자동 삭제
            Destroy(temp, 5f);

            //총알 생성 위치를 머즐 입구로 한다.
            temp.transform.position = start.transform.position;

            //총알의 방향을 오브젝트의 방향으로 한다.
            //->해당 오브젝트가 오브젝트가 360도 회전하고 있으므로, Rotation이 방향이 됨.
            temp.transform.rotation = transform.rotation;
        }
    }

    void Follow()
    {
        GameObject temp = Instantiate(Bullet);

        Destroy(temp, 5f);

        temp.transform.position = start.transform.position;

        Vector2 dirVec = player.transform.position - transform.position;
        temp.transform.rotation = Quaternion.Euler(dirVec.normalized);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            anim.SetBool("Think", true);
            Invoke("Follow", 2f);
        }
        else
        {
            Invoke("Think", 2f);
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
            anim.SetBool("Think", true);
            Invoke("Gun", 2f);
        }
        else
        {
            Invoke("Think", 2f);
        }
    }

    void Arc()
    {
        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            anim.SetBool("Think", true);
            Invoke("Arc", 2f);
        }
        else
        {
            Invoke("Think", 2f);
        }
    }
}
