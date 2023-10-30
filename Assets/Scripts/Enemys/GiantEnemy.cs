using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GiantEnemyState
{
    Wander,         // ������ ���� (= Idle ����)
    // Follow,      // �÷��̾ ������� ����
    Mount,          // �÷��̾ ã���� ���� �÷� ���� ���� ����
    Die             // ����
    // Attack�� OnTrigger�� ����
}

public class GiantEnemy : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;

    private GiantEnemyState curState = GiantEnemyState.Wander;

    private int curHealth;          // �� ���� ���� ü��

    [SerializeField]
    private int maxHealth = 15;          // �� ���� �ִ� ü��

    [SerializeField]
    private float range;         // �÷��̾ �����ϴ� ����

    [SerializeField]
    private float speed;        // �̵� �ӵ�

    [SerializeField]
    private float attackCool;    // ���� ��Ÿ��

    [SerializeField][Range(0, 100)]
    private int ranPotionDropPercent = 25;

    [SerializeField]
    private GameObject potionPrefab;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Slider hpSlider;

    private bool chooseDir = false;
    private bool coolDownAttack = false;
    private Vector3 randomDir;
    private int rndNum;

    private float enemyHeight;
    private float mapHeight;
    private bool mounted;

    [SerializeField]
    private Transform map;

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
        enemyHeight = sprite.bounds.size.y;
        mapHeight = map.localScale.y;
    } 

    private void FixedUpdate()
    {
        switch(curState)
        {
            case GiantEnemyState.Wander:
                Wander();
                break;
            /*            case GiantEnemyState.Follow:
                            Follow();
                            break;*/

            case GiantEnemyState.Mount:
                Mount();
                break;

            case GiantEnemyState.Die:
                break;
        }

        // �����ȿ� �÷��̾ �ְ�, ���� ���� �ʾҴٸ�
        if (IsPlayerInRange(range) && curState != GiantEnemyState.Die)
        {
            curState = GiantEnemyState.Mount;
        } 

        else if (!IsPlayerInRange(range) && curState != GiantEnemyState.Die)
        {
            curState = GiantEnemyState.Wander;
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
            curState = GiantEnemyState.Wander;

        if (IsPlayerInRange(range))
        {
            curState = GiantEnemyState.Mount;
        } 
    }

    // ��� ����
    /*void Follow()
    {
        *//* transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);*//*

        Vector2 newPosition = Vector2.MoveTowards(rigid.position, player.position, speed * Time.deltaTime);

        sprite.flipX = player.position.x > transform.position.x;

        rigid.MovePosition(newPosition);
    }*/

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
        }
        else
        {
            SoundManager.instance.PlaySoundEffect("����������");
        }
    }

    private void Mount()
    {
        animator.SetBool("attack", true);
        if (mounted) return;

        mounted = true;

        int rowCount = Mathf.FloorToInt(mapHeight / enemyHeight);

        for (int i=0; i< rowCount; i++)
        {
            Vector3 spawnPosition = new Vector3(0, i * enemyHeight - mapHeight / 2, 0);
            Instantiate(this.gameObject, spawnPosition, Quaternion.identity);
        }
    }

    public void Death()
    {
        Debug.Log("�𷡰��� ���!");

        // 25���� Ȯ���� �÷��̾� ü�� 1ȸ��
        int ran = Random.Range(0, 100);
 
        if (ran <= ranPotionDropPercent)
        {
            Debug.Log("ü��ȸ��!");
            GameObject hpPotion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
            hpPotion.GetComponent<DropedPotion>().healHP = (int)(Random.Range(1, 8));    // 1~8���� ���� ü�� ȸ��
        } else
        {
            Debug.Log("���� ������. ������ ��� ����");
        }

        Destroy(gameObject);

        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
