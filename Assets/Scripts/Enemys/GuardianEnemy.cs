using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GuardianEnemyState
{
    Wander,     // ������ ���� (= Idle ����)
    Follow,     // �÷��̾ ������� ����
    Die         // ����
    // Attack�� OnTrigger�� ����
}

public class GuardianEnemy : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;

    [SerializeField]
    private GuardianEnemyState curState = GuardianEnemyState.Wander;

    private int curHealth;          // �� ���� ���� ü��

    [SerializeField]
    private int maxHealth = 10;          // �� ���� �ִ� ü��

    [SerializeField]
    private float range = 10f;         // �÷��̾ �����ϴ� ����

    [SerializeField]
    private float speed = 1.3f;        // �̵� �ӵ�

    [SerializeField]
    private float attackCool = 2f;    // ���� ��Ÿ��

    [SerializeField]
    private Transform attackTransform;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float attackDelay = 0.25f;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Slider hpSlider;

    private bool chooseDir = false;
    private bool coolDownAttack = false;
    private Vector3 randomDir;
    private int rndNum;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // ���� ��Ʈ�� �Ҵ� ���� ã���� ������ ���Ƿ� Start���� ����
    private void Start()
    {
        player = GameController.instance.player.transform;
        curHealth = maxHealth;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = curHealth;
    }

    private void FixedUpdate()
    {
        switch (curState)
        {
            case GuardianEnemyState.Wander:
                animator.SetBool("move", true);
                Wander();
                break;
            case GuardianEnemyState.Follow:
                animator.SetBool("move", false);
                Follow();
                break;
            case GuardianEnemyState.Die:
                break;
        }

        // �����ȿ� �÷��̾ �ְ�, ���� ���� �ʾҴٸ�
        if (IsPlayerInRange(range) && curState != GuardianEnemyState.Die)
        {
            curState = GuardianEnemyState.Follow;
        }

        else if (!IsPlayerInRange(range) && curState != GuardianEnemyState.Die)
        {
            curState = GuardianEnemyState.Wander;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        rndNum = Random.Range(0, 1);
        randomDir = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
        chooseDir = false;
    }

    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        if (rndNum == 0)
        {
            transform.position += randomDir * speed * Time.fixedDeltaTime;
            if (randomDir.x < 0)
                sprite.flipX = true;
            else
                sprite.flipX = false;
        }

        else if (rndNum == 1)
            curState = GuardianEnemyState.Wander;

        if (IsPlayerInRange(range))
        {
            curState = GuardianEnemyState.Follow;
        }
    }

    void Follow()
    {
        // transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        Vector2 newPosition = Vector2.MoveTowards(rigid.position, player.position, speed * Time.deltaTime);

        sprite.flipX = player.position.x < transform.position.x;      

        rigid.MovePosition(newPosition);

        speed = 0.7f;      // �̵� �ӵ� �ణ ������

        Invoke("Attack", attackDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameController.instance.DamagePlayer(1);
            
            // ��� ����
            // collision.GetComponent<Player>().StartKnockBack(transform.position);
        }
    }

    // ������ ���Ÿ� ����
    void Attack()
    {
        if (!coolDownAttack)
        {
            if (player.position.x - transform.position.x < 0)
            {
                attackTransform.localPosition = new Vector3(-0.25f, -0.2f, 0f);
            }

            else
            {
                attackTransform.localPosition = new Vector3(0.25f, -0.2f, 0f);
            }

            GameObject prefab = Instantiate(bulletPrefab, attackTransform.position, Quaternion.identity);
            Bullet bullet = prefab.GetComponent<Bullet>();
            bullet.GetPlayer(player.transform);

            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(attackCool);
        coolDownAttack = false;
    }

    public void Damaged()
    {       
        curHealth -= 1;
        hpSlider.value = curHealth;

        if (!canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(true);
        }

        if (curHealth <= 0)
        {
            SoundManager.instance.PlaySoundEffect("�߰������");
            Death();
        } else
        {
            SoundManager.instance.PlaySoundEffect("����������");
        }
    } 

    public void Death()
    {
        Destroy(gameObject);
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
