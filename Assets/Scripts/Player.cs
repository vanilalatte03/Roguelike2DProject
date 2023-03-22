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
        /*
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector2.right * speed);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(-Vector2.right * speed);
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector2.up * speed);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector2.up * speed);
        */

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 CurPos = transform.position;
        Vector2 ChaPos = new Vector2(h, v) * speed * Time.deltaTime;
        transform.position = CurPos + ChaPos;

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
        if (x != 0) y = 0;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y) * bulletSpeed;
    }
}