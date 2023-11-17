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

    //발사될 총알 오브젝트이다.
    public GameObject Bullet;

    public float SpawnInterval;
    private float _spawnTimer;

    void Awake()
    {
        player = GameController.instance.player.transform;
        spinCnt = 0;
        Invoke("Starting", 2f); // 보스 등장 전에 몇초 기다릴지 설정 가능
    }

    void Starting()
    {
        anim.SetTrigger("ChangeAnimation");
        Invoke("Attack", 3f);
    }

    void Attack()
    {
        SoundManager.instance.PlaySoundEffect("스핑크스공격");
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
            //총알 생성
            GameObject temp = Instantiate(Bullet);

            //2초마다 삭제
            Destroy(temp, 5f);

            //총알 생성 위치를 (0,0) 좌표로 한다.
            temp.transform.position = start.transform.position + new Vector3(0.2f, 0, 0);

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            SoundManager.instance.PlaySoundEffect("스핑크스공격");
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
        temp.transform.position = start.transform.position + new Vector3(0.2f, 0, 0);

        //총알의 방향을 오브젝트의 방향으로 한다.
        //->해당 오브젝트가 오브젝트가 360도 회전하고 있으므로, Rotation이 방향이 됨.
        temp.transform.rotation = transform.rotation;
    }

    void Gun()
    {
        float rnd = Random.Range(0f, 360f);

        for (int i = 0; i < 5; i++)
        {
            //총알 생성
            GameObject temp = Instantiate(Bullet);

            //2초마다 삭제
            Destroy(temp, 5f);

            //총알 생성 위치를 (0,0) 좌표로 한다.
            temp.transform.position = start.transform.position + new Vector3(0.2f, 0, 0);

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            temp.transform.rotation = Quaternion.Euler(0, 0, rnd - (i * 10));
        }


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            SoundManager.instance.PlaySoundEffect("스핑크스공격");
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

        //총알 생성
        GameObject temp = Instantiate(Bullet);

        //2초마다 삭제
        Destroy(temp, 5f);

        //총알 생성 위치를 (0,0) 좌표로 한다.
        temp.transform.position = start.transform.position + new Vector3(0.2f, 0, 0);

        //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
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
