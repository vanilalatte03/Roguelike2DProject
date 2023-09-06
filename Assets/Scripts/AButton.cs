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
        
        // (모바일의 공격 UI 클릭 또는 키보드의 a키가 눌려거나) && 쿨타임이 0일 때 공격
        // 하지만 키보드 a 공격은 아직 수정 필요
        if ((isTouch || isApressed) && currentTime <= 0)
        {
            // 보강 필요
            // 모바일 터치일 때는 조이스틱을 가져오고, 키보드 입력일 때는 플레이어의 direction을 가져온다.
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
                      // 보강 필요
                      // angle = 
                  }*/



            //x,y의 값을 조합하여 Z방향 값으로 변형함. -> ~도 단위로 변형
            if (isTouch)
            {
                Vector2 playerDir = joyStick.GetComponent<VariableJoystick>().AttackDir;
                float angle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
                GameObject bullet = Instantiate(bulletPrefab, player.transform.position, Quaternion.Euler(0, 0, angle - 45));
                bullet.GetComponent<Rigidbody2D>().velocity = playerDir * bulletSpeed;
                currentTime = coolTime;
            }

            // 키보드 a눌렀을 때의 전용 화살
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
