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
    private bool isApressed;

    void Start()
    {
        isTouch = false;
        isApressed = false;
    }

    void Update()
    {
        isApressed = Input.GetKeyDown(KeyCode.A);

        coolTime = GameController.FireRate;
        
        // (������� ���� UI Ŭ�� �Ǵ� Ű������ aŰ�� �����ų�) && ��Ÿ���� 0�� �� ����
        // ������ Ű���� a ������ ���� ���� �ʿ�
        if ((isTouch || isApressed) && currentTime <= 0)
        {
            // ���� �ʿ�
            // ����� ��ġ�� ���� ���̽�ƽ�� ��������, Ű���� �Է��� ���� �÷��̾��� direction�� �����´�.
            /*      Vector2 playerDir = new Vector2();
                  float angle = 0;

                  if (isTouch)
                  {
                      playerDir = joyStick.GetComponent<VariableJoystick>().AttackDir;
                      angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
                  }

                  else if (isApressed)
                  {
                      playerDir = player.GetComponent<Player>().transform.rotation.eulerAngles;
                      // ���� �ʿ�
                      // angle = 
                  }*/



            //x,y�� ���� �����Ͽ� Z���� ������ ������. -> ~�� ������ ����
            if (isTouch)
            {
                Vector2 playerDir = joyStick.GetComponent<VariableJoystick>().AttackDir;
                float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
                GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.Euler(0, 0, angle - 45));
                bullet.GetComponent<Rigidbody2D>().velocity = playerDir * bulletSpeed;
                currentTime = coolTime;
            }

            // Ű���� a������ ���� ���� ȭ��
            else if (isApressed)
            {
                Vector2 playerDir = player.GetComponent<Player>().resultVec;
                float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
                GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.Euler(0, 0, angle - 45));
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * bulletSpeed;
                currentTime = coolTime;
            }
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
