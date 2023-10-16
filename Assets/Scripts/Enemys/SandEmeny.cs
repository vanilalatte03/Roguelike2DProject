using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiddleBossState
{
    Wander,     // ������ ���� (= Idle ����)
    Follow,     // �÷��̾ ������� ����
    Die         // ����
    // Attack�� OnTrigger�� ����
}

public class SandEmeny : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer sprite;

    [SerializeField]
    private MiddleBossState curState = MiddleBossState.Wander;

    [SerializeField]
    private int curHealth;          // �� ���� ���� ü��

    [SerializeField]
    private int maxHealth = 20;          // �� ���� �ִ� ü��

    [SerializeField]
    private float range;         // �÷��̾ �����ϴ� ����

    [SerializeField]
    private float speed;        // �̵� �ӵ�

    [SerializeField]
    private float atackCool;    // ���� ��Ÿ��

    private bool chooseDir = false;
    private bool coolDownAttack = false;
    private Vector3 randomDir;
    private int rndNum;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // ���� ��Ʈ�� �Ҵ� ���� ã���� ������ ���Ƿ� Start���� ����
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

        // �����ȿ� �÷��̾ �ְ�, ���� ���� �ʾҴٸ�
        if (IsPlayerInRange(range) && curState != MiddleBossState.Die)
        {
            Debug.Log("�÷��̾� ����!");
            curState = MiddleBossState.Follow;
        } 

        else if (!IsPlayerInRange(range) && curState != MiddleBossState.Die)
        {
            Debug.Log("�÷��̾� ����..");
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
            Debug.Log("�÷��̾� ����!");
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
        SoundManager.instance.PlaySoundEffect("�����1");

        if (curHealth >= 2)
        {
            curHealth -= 1;            
            Debug.Log("���� ü�� : " + curHealth);
        } else
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
