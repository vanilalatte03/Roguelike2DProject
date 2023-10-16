using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiddleBossState
{
    Wander,     // 떠도는 상태 (= Idle 상태)
    Follow,     // 플레이어를 따라오는 상태
    Die         // 죽음
    // Attack은 OnTrigger로 구현
}

public class SandEmeny : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer sprite;

    [SerializeField]
    private MiddleBossState curState = MiddleBossState.Wander;

    [SerializeField]
    private int curHealth;          // 모래 거인 현재 체력

    [SerializeField]
    private int maxHealth = 20;          // 모래 거인 최대 체력

    [SerializeField]
    private float range;         // 플레이어를 인지하는 범위

    [SerializeField]
    private float speed;        // 이동 속도

    [SerializeField]
    private float atackCool;    // 공격 쿨타임

    private bool chooseDir = false;
    private bool coolDownAttack = false;
    private Vector3 randomDir;
    private int rndNum;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // 게임 컨트롤 할당 전에 찾으면 오류가 나므로 Start에서 선언
    private void Start()
    {       
        player = GameController.instance.player.transform;
        curHealth = maxHealth;
    } 

    private void FixedUpdate()
    {
        switch(curState)
        {
            case MiddleBossState.Wander:
                Wander();
                break;     
            case MiddleBossState.Follow:
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
            sprite.flipX = false;
        else
            sprite.flipX = true;

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
                sprite.flipX = false;
            else
                sprite.flipX = true;
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
        // animator.SetBool("Idle", false);

        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (!coolDownAttack)
        {
            GameController.instance.DamagePlayer(1);
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
        } else
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
