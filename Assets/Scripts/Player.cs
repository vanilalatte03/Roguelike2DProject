using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float coolTime;
    private float currentTime;
    private int direction = 1;
    private int shootDir = -45;

    void Start()
    {
       
    }

    void Update()
    {
         //transform.localScale = new Vector3(direction, 1, 1); // 스프라이트 회전
   
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