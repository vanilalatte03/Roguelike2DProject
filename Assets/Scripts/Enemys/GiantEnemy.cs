using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GiantEnemyState
{
    Wander,         // 떠도는 상태 (= Idle 상태)
    // Follow,      // 플레이어를 따라오는 상태
    Mount,          // 플레이어를 찾으면 몸을 늘려 벽을 막는 상태
    Die             // 죽음
    // Attack은 OnTrigger로 구현
}

public class GiantEnemy : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;

    private GiantEnemyState curState = GiantEnemyState.Wander;

    private int curHealth;          // 모래 거인 현재 체력

    [SerializeField]
    private int maxHealth = 15;          // 모래 거인 최대 체력

    [SerializeField]
    private float range;         // 플레이어를 인지하는 범위

    [SerializeField]
    private float speed;        // 이동 속도

    [SerializeField]
    private float attackCool;    // 공격 쿨타임

    [SerializeField][Range(0, 100)]
    private int ranPotionDropPercent = 25;

    [SerializeField]
    private GameObject potionPrefab;

    [SerializeField]
    private GameObject copyedGiantEnemy;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Slider hpSlider;

    private bool chooseDir = false;
    private bool coolDownAttack = false;
    private Vector3 randomDir;
    private int rndNum;

    [SerializeField]
    private float mapHeight = 8.5f;            // 맵의 높이를 가져오는 방법을 모르겠어서 대충 짐작해서 8.5로 적용함. 추후 수정 가능

    private float enemyHeight;
    private bool mounted;

    [SerializeField]
    private bool isCopyed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // 게임 컨트롤 할당 전에 찾으면 오류가 나므로 Start에서 선언
    private void Start()
    {
        // 분신이면 그냥 start함수에서 바로 적용 (애니만 있음)
        if (isCopyed)
        {
            curState = GiantEnemyState.Wander;
        }
        else
        {
            player = GameController.instance.player.transform;
            enemyHeight = sprite.bounds.size.y;
        }

        curHealth = maxHealth;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = curHealth;
    } 

    private void FixedUpdate()
    {
        switch(curState)
        {
            case GiantEnemyState.Wander:
                animator.SetBool("move", true);

                if (!isCopyed)
                {
                    Wander();
                } 

                break;
            /*            case GiantEnemyState.Follow:
                            Follow();
                            break;*/

            case GiantEnemyState.Mount:
                animator.SetBool("move", false);
                Mount();
                break;

            case GiantEnemyState.Die:
                break;
        }

        // 분신 몬스터는 아래 로직을 수행하지 않는다
        if (isCopyed) return;

        // 범위안에 플레이어가 있고, 현재 죽지 않았다면
        if (IsPlayerInRange(range) && curState != GiantEnemyState.Die)
        {
            curState = GiantEnemyState.Mount;
        } 

        // 이미 한번 마운트했으면 더이상 wander하지 않는다.
        else if (!IsPlayerInRange(range) && curState != GiantEnemyState.Die && !mounted)
        {
            curState = GiantEnemyState.Wander;
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
                sprite.flipX = false;
            else
                sprite.flipX = true;
        }

        else if (rndNum == 1)
            curState = GiantEnemyState.Wander;

        if (IsPlayerInRange(range))
        {
            curState = GiantEnemyState.Mount;
        } 
    }

    // 거인 몹은 따라다니지 않으므로 생략
    /*void Follow()
    {
        *//* transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);*//*

        Vector2 newPosition = Vector2.MoveTowards(rigid.position, player.position, speed * Time.deltaTime);

        sprite.flipX = player.position.x > transform.position.x;

        rigid.MovePosition(newPosition);
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
            SoundManager.instance.PlaySoundEffect("중간몹사망");
            Death();
        }
        else
        {
            SoundManager.instance.PlaySoundEffect("적피해입음");
        }
    }

    private void Mount()
    {
        if (mounted) return;

        mounted = true;

        Debug.Log("mapHeight" + mapHeight);
        SoundManager.instance.PlaySoundEffect("모래거인길막");
        float startY = -mapHeight / 2 + enemyHeight / 2;

        // 분신을 생성하며, 분신의 높이가 맵의 전체 높이를 초과하지 않도록 함       
        for (float y = startY; y <= mapHeight / 2; y += enemyHeight)
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            Vector3 spawnPosition = new Vector3(transform.position.x, y, 0);
            GameObject copyed = Instantiate(copyedGiantEnemy, spawnPosition, Quaternion.identity);
            copyed.GetComponent<SpriteRenderer>().flipX = transform.position.x < player.position.x;
        }
    
    }

    public void Death()
    {
        Debug.Log("모래거인 사망!");

        // 25퍼의 확률로 플레이어 체력 1회복
        int ran = Random.Range(0, 100);
 
        if (ran <= ranPotionDropPercent)
        {
            Debug.Log("체력회복!");
            GameObject hpPotion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
            hpPotion.GetComponent<DropedPotion>().healHP = (int)(Random.Range(1, 8));    // 1~8사이 랜덤 체력 회복
        } else
        {
            Debug.Log("운이 없군요. 아이템 드랍 실패");
        }

        Destroy(gameObject);

        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
