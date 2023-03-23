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
    private int direction = 1;
    private int shootDir = -45;

    void Start()
    {
        
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h < 0) direction = -1;
        else if (h > 0) direction = 1;
        Vector2 CurPos = transform.position;
        Vector2 ChaPos = new Vector2(h, v) * speed * Time.deltaTime;
        transform.localScale = new Vector3(direction, 1, 1);
        transform.position = CurPos + ChaPos;

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