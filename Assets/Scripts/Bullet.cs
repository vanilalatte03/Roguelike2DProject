using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false;
    public bool isBossBullet = false;
    public bool isGuardianBullet = false;

    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector2 playerPos;

    [SerializeField]
    private float speed = 5.0f;                      // guardianEnemy 유도탄 속도
    
    [SerializeField]
    private float rotationSpeed = 200.0f;            // guardianEnemy 유도탄 회전 속도

    [SerializeField]
    private GameObject spriteObject;                 // (가디언 몬스터 전용) sprite는 그냥 두기 위해서, 다른 타겟 트랜스폼을 둔다.


    void Start()
    {
        StartCoroutine(DeathDelay());
        if (!isEnemyBullet)
        {
            // transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    void Update()
    {
        if (isEnemyBullet)
        {
            curPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            
            if (curPos == lastPos)
            {
                Destroy(gameObject);
            }
            lastPos = curPos;
        }

        // 가디언 몬스터 불렛일 때
        CheckGuardianBullet();

        if (isBossBullet)
        {
            //두번째 파라미터에 Space.World를 해줌으로써 Rotation에 의한 방향 오류를 수정함
            transform.Translate(Vector2.right * (5f * Time.deltaTime), Space.Self);
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // 플레이어가 쏘는 화살
        if (col.tag == "Enemy" && !isEnemyBullet)
        {
            col.gameObject.GetComponent<EnemyController>().Damage();
            if (col.gameObject.GetComponent<EnemyController>().health <= 0)
                col.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        // 플레이어가 쏘는 화살이 스핑크스에게 맞았으면
        else if (col.tag == "SphinxEnemy" && !isBossBullet)
        {
            col.GetComponent<SphinxEnemy>().Damaged();
            Destroy(gameObject);
        }

        // 플레이어가 쏘는 화살이 가디언 불렛에게 맞았으면
        else if (col.tag == "GuardianEnemy" && !isGuardianBullet)
        {
            col.GetComponent<GuardianEnemy>().Damaged();
            Destroy(gameObject);
        }

        // 플레이어가 쏘는 화살이 Warm에 맞았으면
        else if (col.tag == "WarmEnemy")
        {
            col.GetComponent<WarmEnemy>().Damaged();
            Destroy(gameObject);
        }

        // 플레이어가 쏘는 화살
        else if ((col.tag == "GiantEnemy") && (!isEnemyBullet && !isGuardianBullet))
        {
            col.GetComponent<GiantEnemy>().Damaged();
            Destroy(gameObject);
        }

        // 일반 enemy가 쏘는 화살
        else if (col.tag == "Player" && isEnemyBullet)
        {
            GameController.instance.DamagePlayer(1);
            Destroy(gameObject);
        }

        // boss가 쏘는 화살
        else if (col.tag == "Player" && isBossBullet)
        {
            GameController.instance.DamagePlayer(2);
            Destroy(gameObject);
        }

        // 일반 guardianEnemy가 쏘는 화살
        else if (col.tag == "Player" && isGuardianBullet)
        {
            GameController.instance.DamagePlayer(2);     
            Destroy(gameObject);
        }
    }

    public void CheckGuardianBullet()
    {
        if (isGuardianBullet)
        {
            if (playerPos == null)
            {
                Destroy(gameObject);
                return;
            }
        
            Vector2 bulletPos = new Vector2(transform.position.x, transform.position.y); // Vector2로 변환

            Vector2 direction = (playerPos - bulletPos).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 유도탄을 플레이어 방향으로 이동
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            if (spriteObject != null)
            {
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }  
        }
    }
}
