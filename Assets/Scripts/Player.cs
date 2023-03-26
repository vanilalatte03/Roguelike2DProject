using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    Rigidbody2D rigidbody;

    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float coolTime;
    private float currentTime;
    private int direction = 1;
    private int shootDir = -45;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical"); // 수직 및 수평으로 이동하는 걸 받아옴. 
        if (h < 0) direction = -1;
        else if (h > 0) direction = 1;
  
        rigidbody.velocity = new Vector3(h * speed, v * speed, 0);// 캐릭터 이동 구문. 
        transform.localScale = new Vector3(direction, 1, 1); // 스프라이트 회전
   
        float shootH = Input.GetAxisRaw("ShootH");
        float shootV = Input.GetAxisRaw("ShootV");
        if (shootH < 0) shootDir = 135;
        else if (shootH > 0) shootDir = -45;
        else if (shootV < 0) shootDir = -135;
        else if (shootV > 0) shootDir = 45;
        if((shootH != 0 || shootV != 0) && currentTime <= 0)
        {
            Shoot(shootH, shootV);

            currentTime = coolTime;
        }
        currentTime -= Time.deltaTime;
    }

    void Shoot(float x, float y)
    {
        if (x != 0) y = 0;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, shootDir)));
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y) * bulletSpeed;
    }
}