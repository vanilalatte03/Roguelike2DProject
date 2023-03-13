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
        // 이거 Switch 문으로 못바꾸나?
        // 내가 Project Setting에서 Horizontal이랑 Vertical 설정 바꿔놔서
        // 이동도 그냥 GetAxis로 Horizontal/Vertical 사용해서 구현해도 될 것 같아

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
        