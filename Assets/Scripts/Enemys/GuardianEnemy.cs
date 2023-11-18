using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GuardianState
{
    Wander,     // 떠도는 상태 (= Idle 상태)
    Follow,     // 플레이어를 따라오는 상태
    Die         // 죽음
    // Attack은 OnTrigger로 구현
}

public class GuardianEnemy : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;

    [SerializeField]
    private GuardianState curState = GuardianState.Wander;

    private int curHealth;          // 모래 거인 현재 체력

    [SerializeField]
    private int maxHealth;          // 모래 거인 최대 체력

    [SerializeField]
    private float range;         // 플레이어를 인지하는 범위

    [SerializeField]
    private float speed;        // 이동 속도

    [SerializeField]
    private Transform attackTransform;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Slider hpSlider;

    private bool chooseDir = false;
    private Vector3 randomDir;
    private int rndNum;
    public bool notInRoom;

    [SerializeField]
    private GameObject destoryAnimObj;

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
        hpSlider.maxValue = maxHealth;
        hpSlider.value = curHealth;
    }

    private void FixedUpdate()
    {
        if (notInRoom) curState = GuardianState.Wander;

        switch (curState)
        {
            case GuardianState.Wander:
                animator.SetBool("move", true);
                if (notInRoom) return;
                Wander();
                break;

            case GuardianState.Follow:
                animator.SetBool("move", false);
                if (notInRoom) return;
                Follow();
                break;

            case GuardianState.Die:
                break;
        }

        // 범위안에 플레이어가 있고, 현재 죽지 않았다면
        if (IsPlayerInRange() && curState != GuardianState.Die)
        {
            curState = GuardianState.Follow;
        }

        else if (!IsPlayerInRange() && curState != GuardianState.Die)
        {
            curState = GuardianState.Wander;
        }
    }

    private bool IsPlayerInRange()
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
            curState = GuardianState.Wander;

        if (IsPlayerInRange())
        {
            curState = GuardianState.Follow;
        }
    }

    void Follow()
    {
        // transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        Vector2 newPosition = Vector2.MoveTowards(rigid.position, player.position, speed * Time.deltaTime);

        sprite.flipX = player.position.x < transform.position.x;      

        rigid.MovePosition(newPosition);

        speed = 0.7f;      // 이동 속도 약간 느리게

        // Attack(); 애니메이션 key event로 대체
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.DamagePlayer(1);

            collision.gameObject.GetComponent<Player>().StartKnockBack(transform.position);
        }
    }

    // 공격은 원거리 공격, 애니메이션에서 호출할 예정
    public void Attack()
    {
        /*if (!coolDownAttack)
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
        }*/

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
        SoundManager.instance.PlaySoundEffect("가디언공격");
    }

    // 애니메이션에서 호출하므로 필요없음
/*    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(attackCool);
        coolDownAttack = false;
    }*/

    public void Damaged(int dm)
    {       
        curHealth -= dm;
        hpSlider.value = curHealth;

        if (!canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(true);
        }

        if (curHealth <= 0)
        {
            SoundManager.instance.PlaySoundEffect("중간몹사망");
            Death();
        } else
        {
            SoundManager.instance.PlaySoundEffect("적피해입음");
        }
    } 

    public void Death()
    {
        Instantiate(destoryAnimObj, transform.position, Quaternion.identity);
        curState = GuardianState.Die;
        Destroy(gameObject);
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
