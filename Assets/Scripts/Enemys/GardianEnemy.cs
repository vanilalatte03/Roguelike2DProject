using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardianEnemy : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;

    [SerializeField]
    private MiddleBossState curState = MiddleBossState.Wander;

    [SerializeField]
    private int curHealth;          // 모래 거인 현재 체력

    [SerializeField]
    private int maxHealth = 10;          // 모래 거인 최대 체력

    [SerializeField]
    private float range = 10f;         // 플레이어를 인지하는 범위

    [SerializeField]
    private float speed = 1.3f;        // 이동 속도

    [SerializeField]
    private float atackCool = 2f;    // 공격 쿨타임

    [SerializeField]
    private Transform attackTransform;

    [SerializeField]
    private GameObject bulletPrefab;

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

    // 게임 컨트롤 할당 전에 찾으면 오류가 나므로 Start에서 선언
    private void Start()
    {
        player = GameController.instance.player.transform;
        curHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        switch (curState)
        {
            case MiddleBossState.Wander:
                animator.SetBool("move", true);
                Wander();
                break;
            case MiddleBossState.Follow:
                animator.SetBool("move", false);
                Follow();
                break;
            case MiddleBossState.Die:
                break;
        }

        // 범위안에 플레이어가 있고, 현재 죽지 않았다면
        if (IsPlayerInRange(range) && curState != MiddleBossState.Die)
        {
            Debug.Log("플레이어 포착!");
            curState = MiddleBossState.Follow;
        }

        else if (!IsPlayerInRange(range) && curState != MiddleBossState.Die)
        {
            Debug.Log("플레이어 잃음..");
            curState = MiddleBossState.Wander;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        if (player.position.x - transform.position.x < 0)
            sprite.flipX = true;
        else
            sprite.flipX = false;

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
            if (randomDir.x - transform.position.x < 0)
                sprite.flipX = true;
            else
                sprite.flipX = false;
        }

        else if (rndNum == 1)
            curState = MiddleBossState.Wander;

        if (IsPlayerInRange(range))
        {
            Debug.Log("플레이어 포착!");
            curState = MiddleBossState.Follow;
        }
    }

    void Follow()
    {
        // transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        Vector2 newPosition = Vector2.MoveTowards(rigid.position, player.position, speed * Time.deltaTime);
        rigid.MovePosition(newPosition);

        Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameController.instance.DamagePlayer(1);
        }
    }

    // 공격은 원거리 공격
    void Attack()
    {
        if (!coolDownAttack)
        {
            GameObject prefab = Instantiate(bulletPrefab, attackTransform.position, Quaternion.identity);
            Bullet bullet = prefab.GetComponent<Bullet>();
            bullet.GetPlayer(player.transform);
            bullet.isEnemyBullet = true;
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(atackCool);
        coolDownAttack = false;
    }

    public void Damaged()
    {
        SoundManager.instance.PlaySoundEffect("적사망1");

        if (curHealth >= 2)
        {
            curHealth -= 1;
            Debug.Log("현재 체력 : " + curHealth);
        }
        else
        {
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("모래거인 사망!");
        Destroy(gameObject);
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
