using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false;
    public bool isBossBullet = false;

    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector2 playerPos;
    void Start()
    {
        StartCoroutine(DeathDelay());
        if (!isEnemyBullet)
        {
            //transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
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
        if (col.tag == "Enemy" && !isEnemyBullet)
        {
            col.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        if (col.tag == "Player" && isEnemyBullet)
        {
            GameController.instance.DamagePlayer(1);
            Destroy(gameObject);
        }
    }
}
