using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class AButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject bulletPrefab;
    public GameObject joyStick;
    public GameObject player;
    public float bulletSpeed;
    public float coolTime;
    private float currentTime;
    private bool isTouch;

    void Start()
    {
        isTouch = false;
    }

    void Update()
    {
        coolTime = GameController.FireRate;

        if (isTouch && currentTime <= 0)
        {
            Vector2 playerDir = joyStick.GetComponent<VariableJoystick>().AttackDir;
            //x,y의 값을 조합하여 Z방향 값으로 변형함. -> ~도 단위로 변형
            float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.Euler(0, 0, angle - 45));
            bullet.GetComponent<Rigidbody2D>().velocity = playerDir * bulletSpeed;
            currentTime = coolTime;
        }
        currentTime -= Time.deltaTime;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
    }
}
