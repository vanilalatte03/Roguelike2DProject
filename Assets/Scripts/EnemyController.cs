using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Slider hpSlider;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;
    public float range;
    public float speed;
    public int maxHealth;
    public int health;
    public float attackRange;
    public float coolDown;
    private bool chooseDir = false;
    private bool coolDownAttack = false;
    public bool notInRoom = false;
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    int rndNum;
    [SerializeField]
    private bool isSlime;           // 슬라임 몬스터는 Idle 애니가 없으므로 경고 출력.

    [SerializeField]
    private GameObject destoryAnimObj;


    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        hpSlider.maxValue = maxHealth;
        health = maxHealth;
        hpSlider.value = health;
    }

    void FixedUpdate()
    {
        switch (currState)
        {
            case EnemyState.Idle:
                if (!isSlime) animator.SetBool("Idle", true);
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
            transform.GetComponent<SpriteRenderer>().flipX = true;
        else
            transform.GetComponent<SpriteRenderer>().flipX = false;

        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        rndNum = Random.Range(0, 1);
        randomDir = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)).normalized;
        chooseDir = false;

        // 슬라임은 Idle 애니메이션이 없음
        if (!isSlime)
        {
            animator.SetBool("Idle", false);
        }   
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
                transform.GetComponent<SpriteRenderer>().flipX = true;
            else
                transform.GetComponent<SpriteRenderer>().flipX = false;
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
        if (!isSlime)
        {
            animator.SetBool("Idle", false);
        }   

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
                    // GameController.instance.player.StartKnockBack(transform.position);       // 넉백 효과, 하위몹 계열은 약하므로 넉백 생략
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

    public void Damage()
    {
        health -= 1;
        hpSlider.value = health;

        if (!canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(true);
        }

        if (health <= 0)
        {
            Death();
        } 

        else
        {
            SoundManager.instance.PlaySoundEffect("적피해입음");
        }
    }

    public void Death()
    {
        SoundManager.instance.PlaySoundEffect("일반몹사망");
        Destroy(gameObject);
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
       
    }
}
