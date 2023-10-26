using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SandEnemyState
{
    Wander,     // ������ ���� (= Idle ����)
    Follow,     // �÷��̾ ������� ����
    Die         // ����
    // Attack�� OnTrigger�� ����
}

public class SandEnemy : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;

    private SandEnemyState curState = SandEnemyState.Wander;

    private int curHealth;          // �� ���� ���� ü��

    [SerializeField]
    private int maxHealth = 15;          // �� ���� �ִ� ü��

    [SerializeField]
    private float range;         // �÷��̾ �����ϴ� ����

    [SerializeField]
    private float speed;        // �̵� �ӵ�

    [SerializeField]
    private float attackCool;    // ���� ��Ÿ��

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
        switch(curState)
        {
            case SandEnemyState.Wander:
                Wander();
                break;     
            case SandEnemyState.Follow:
                Follow();
                break;
            case SandEnemyState.Die:
                break;
        }

        // �����ȿ� �÷��̾ �ְ�, ���� ���� �ʾҴٸ�
        if (IsPlayerInRange(range) && curState != SandEnemyState.Die)
        {
            Debug.Log("�÷��̾� ã��");
            curState = SandEnemyState.Follow;
        } 

        else if (!IsPlayerInRange(range) && curState != SandEnemyState.Die)
        {
            Debug.Log("�÷��̾� �� ã��");
            curState = SandEnemyState.Wander;
        }
    }

    private bool IsPlayerInRange(float range)
    {
/*        if (player.position.x - transform.position.x < 0)
            sprite.flipX = false;
        else
            sprite.flipX = true;*/

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
            curState = SandEnemyState.Wander;

        if (IsPlayerInRange(range))
        {
            curState = SandEnemyState.Follow;
        } 
    }

    void Follow()
    {
        /* transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);*/

        Vector2 newPosition = Vector2.MoveTowards(rigid.position, player.position, speed * Time.deltaTime);

        sprite.flipX = player.position.x > transform.position.x;

        rigid.MovePosition(newPosition);
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
        yield return new WaitForSeconds(attackCool);
        coolDownAttack = false;
    }

    public void Damaged()
    {
        SoundManager.instance.PlaySoundEffect("�����1");

        curHealth -= 1;
        hpSlider.value = curHealth;
      
        if (!canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(true);
        }

        if (curHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("�𷡰��� ���!");
        Destroy(gameObject);
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
