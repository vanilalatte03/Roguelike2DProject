using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.Euler(new Vector3(0, 0, -45)));
            bullet.GetComponent<Rigidbody2D>().velocity = playerDir * bulletSpeed;
            currentTime = coolTime;
        }
        currentTime -= Time.deltaTime;


        Debug.Log(joyStick.GetComponent<VariableJoystick>().AttackDir);
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
