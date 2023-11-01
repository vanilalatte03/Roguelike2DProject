using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    //회전되는 스피드이다.
    [SerializeField]
    private GameObject start;
    public float TurnSpeed;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    //발사될 총알 오브젝트이다.
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
        //360번 반복
        for (int i = 0; i < 360; i += 13)
        {
            //총알 생성
            GameObject temp = Instantiate(Bullet);

            //2초마다 삭제
            Destroy(temp, 2f);

            //총알 생성 위치를 (0,0) 좌표로 한다.
            temp.transform.position = start.transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
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
        //기본 회전
        transform.Rotate(Vector3.forward * (TurnSpeed * 100 * Time.deltaTime));

        //생성 간격 처리
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer < SpawnInterval) return;

        //초기화
        _spawnTimer = 0f;

        //총알 생성
        GameObject temp = Instantiate(Bullet);

        //2초후 자동 삭제
        Destroy(temp, 2f);

        //총알 생성 위치를 머즐 입구로 한다.
        temp.transform.position = start.transform.position;

        //총알의 방향을 오브젝트의 방향으로 한다.
        //->해당 오브젝트가 오브젝트가 360도 회전하고 있으므로, Rotation이 방향이 됨.
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
