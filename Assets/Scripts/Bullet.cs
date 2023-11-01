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
    private float speed = 5.0f;                      // guardianEnemy ����ź �ӵ�
    
    [SerializeField]
    private float rotationSpeed = 200.0f;            // guardianEnemy ����ź ȸ�� �ӵ�

    [SerializeField]
    private GameObject spriteObject;                 // (����� ���� ����) sprite�� �׳� �α� ���ؼ�, �ٸ� Ÿ�� Ʈ�������� �д�.


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

        // ����� ���� �ҷ��� ��
        CheckGuardianBullet();

        if (isBossBullet)
        {
            //�ι�° �Ķ���Ϳ� Space.World�� �������ν� Rotation�� ���� ���� ������ ������
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
        // �÷��̾ ��� ȭ��
        if (col.tag == "Enemy" && !isEnemyBullet)
        {
            col.gameObject.GetComponent<EnemyController>().Damage();
            if (col.gameObject.GetComponent<EnemyController>().health <= 0)
                col.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        // �÷��̾ ��� ȭ���� ����ũ������ �¾�����
        else if (col.tag == "SphinxEnemy" && !isBossBullet)
        {
            col.GetComponent<SphinxEnemy>().Damaged();
            Destroy(gameObject);
        }

        // �÷��̾ ��� ȭ���� ����� �ҷ����� �¾�����
        else if (col.tag == "GuardianEnemy" && !isGuardianBullet)
        {
            col.GetComponent<GuardianEnemy>().Damaged();
            Destroy(gameObject);
        }

        // �÷��̾ ��� ȭ���� Warm�� �¾�����
        else if (col.tag == "WarmEnemy")
        {
            col.GetComponent<WarmEnemy>().Damaged();
            Destroy(gameObject);
        }

        // �÷��̾ ��� ȭ��
        else if ((col.tag == "GiantEnemy") && (!isEnemyBullet && !isGuardianBullet))
        {
            col.GetComponent<GiantEnemy>().Damaged();
            Destroy(gameObject);
        }

        // �Ϲ� enemy�� ��� ȭ��
        else if (col.tag == "Player" && isEnemyBullet)
        {
            GameController.instance.DamagePlayer(1);
            Destroy(gameObject);
        }

        // boss�� ��� ȭ��
        else if (col.tag == "Player" && isBossBullet)
        {
            GameController.instance.DamagePlayer(2);
            Destroy(gameObject);
        }

        // �Ϲ� guardianEnemy�� ��� ȭ��
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
        
            Vector2 bulletPos = new Vector2(transform.position.x, transform.position.y); // Vector2�� ��ȯ

            Vector2 direction = (playerPos - bulletPos).normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // ����ź�� �÷��̾� �������� �̵�
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            if (spriteObject != null)
            {
                spriteObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }  
        }
    }
}
