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
            col.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        // �÷��̾ ��� ȭ��
        else if (col.tag == "GuardianEnemy" && !isGuardianBullet)
        {
            col.gameObject.GetComponent<GuardianEnemy>().Damaged();
            Destroy(gameObject);
        }

        // �÷��̾ ��� ȭ��
        else if ((col.tag == "SandEnemy") && (!isEnemyBullet && !isGuardianBullet))
        {
            col.GetComponent<SandEnemy>().Damaged();
            Destroy(gameObject);
        }

        // �Ϲ� enemy�� ��� ȭ��
        else if (col.tag == "Player" && isEnemyBullet)
        {
            GameController.instance.DamagePlayer(1);
            Destroy(gameObject);
        }

        // �Ϲ� guardianEnemy�� ��� ȭ��
        else if (col.tag == "Player" && isGuardianBullet)
        {
            GameController.instance.DamagePlayer(1);     
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
        }
    }
}
