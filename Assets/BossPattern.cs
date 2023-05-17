using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    //ȸ���Ǵ� ���ǵ��̴�.
    public float TurnSpeed;

    //�߻�� �Ѿ� ������Ʈ�̴�.
    public GameObject Bullet;

    public float SpawnInterval = 0.5f;
    private float _spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CircleShot();
        }
        if (Input.GetMouseButtonDown(1))
        {
            SpinShot();
        }
    }

    private void CircleShot()
    {
        //360�� �ݺ�
        for (int i = 0; i < 360; i += 13)
        {
            //�Ѿ� ����
            GameObject temp = Instantiate(Bullet);

            //2�ʸ��� ����
            //Destroy(temp, 2f);

            //�Ѿ� ���� ��ġ�� (0,0) ��ǥ�� �Ѵ�.
            temp.transform.position = transform.position;

            //Z�� ���� ���ؾ� ȸ���� �̷�����Ƿ�, Z�� i�� �����Ѵ�.
            temp.transform.rotation = Quaternion.Euler(0, 0, i);
        }
    }

    private void SpinShot()
    {
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
        temp.transform.position = transform.position;

        //�Ѿ��� ������ ������Ʈ�� �������� �Ѵ�.
        //->�ش� ������Ʈ�� ������Ʈ�� 360�� ȸ���ϰ� �����Ƿ�, Rotation�� ������ ��.
        temp.transform.rotation = transform.rotation;
    }
}
