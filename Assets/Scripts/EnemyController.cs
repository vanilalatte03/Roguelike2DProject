using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,     // 떠도는 상태
    Follow,     // 플레이어를 따라오는 상태
    Die,        // 죽은 상태
    Attack      // 공격
};

public enum EnemyType
{
    Melee,
    Ranged,
};

public class EnemyController : MonoBehaviour
{
    Animator animator;
    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;
    public float range;
    public float speed;
    public float attackRange;
    public float coolDown;
    private bool chooseDir = false;
    private bool coolDownAttack = false;
    public bool notInRoom = false;
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    int rndNum;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        switch (currState)
        {
            case EnemyState.Idle:
                animator.SetBool("Idle", true);
                break;
            case EnemyState.Wander:
                Wander();
                break;
            case EnemyState.Follow:
                Follow();
                break;
            case EnemyState.Die:
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }

        if (!notInRoom)
        {
            if (IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Follow;
            }
            else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currState = EnemyState.Attack;
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        if (player.transform.position.x - transform.position.x < 0)
            transform.localScale = new Vector2(-2, 2);
        else
            transform.localScale = new Vector2(2, 2);

        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        rndNum = Random.Range(0, 1);
        randomDir = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
        chooseDir = false;
        animator.SetBool("Idle", false);
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
                transform.localScale = new Vector2(-2, 2);
            else
                transform.localScale = new Vector2(2, 2);
        }
        else if (rndNum == 1)
            currState = EnemyState.Idle;
        if (IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }
    }

    void Follow()
    {
        animator.SetBool("Idle", false);

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        if (!coolDownAttack)
        {
            switch (enemyType)
            {
                case (EnemyType.Melee):
                    GameController.instance.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                    break;

                case (EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<Bullet>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<Bullet>().isEnemyBullet = true;
                    StartCoroutine(CoolDown());
                    break;           
            }
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    public void Death()
    {
        SoundManager.instance.PlaySoundEffect("일반몹사망");

        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);
    }
}
