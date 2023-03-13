using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float coolTime;
    private float currentTime;

    void Start()
    {
        
    }

    void Update()
    { 
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * speed);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(-Vector2.right * speed);
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector2.up * speed);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector2.up * speed);
        // �̰� Switch ������ ���ٲٳ�?
        // ���� Project Setting���� Horizontal�̶� Vertical ���� �ٲ����
        // �̵��� �׳� GetAxis�� Horizontal/Vertical ����ؼ� �����ص� �� �� ����

        float shootH = Input.GetAxisRaw("ShootH");
        float shootV = Input.GetAxisRaw("ShootV");
        if((shootH != 0 || shootV != 0) && currentTime <= 0)
        {
            Shoot(shootH, shootV);

            currentTime = coolTime;
        }
        currentTime -= Time.deltaTime;
    }
    
    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y) * bulletSpeed;
    }
}
        