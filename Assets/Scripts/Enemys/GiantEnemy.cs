using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GiantState
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

    private GiantState curState = GiantState.Wander;

    private int curHealth;          // �� ���� ���� ü��

    [SerializeField]
    private int maxHealth = 15;          // �� ���� �ִ� ü��

    [SerializeField]
    private float range;         // �÷��̾ �����ϴ� ����

    [SerializeField]
    private float speed;        // �̵� �ӵ�

    [SerializeField]
    private float attackCool;    // ���� ��Ÿ��

    [Range(0, 100)]
    public int ranPotionDropPercent = 25;

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
    private float mapHeight = 8.5f;            // ���� ���̸� �������� ����� �𸣰ھ ���� �����ؼ� 8.5�� ������. ���� ���� ����

    private float enemyHeight;
    private bool mounted;

    [SerializeField]
    private bool isCopyed;

    [SerializeField]
    private GameObject destoryAnimObj;

    public bool notInRoom;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // ���� ��Ʈ�� �Ҵ� ���� ã���� ������ ���Ƿ� Start���� ����
    private void Start()
    {
        // �н��̸� �׳� start�Լ����� �ٷ� ���� (�ִϸ� ����)
        if (isCopyed)
        {
            curState = GiantState.Wander;
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
            case GiantState.Wander:
                animator.SetBool("move", true);

                if (!isCopyed)
                {
                    Wander();
                } 

                break;
            /*            case GiantState.Follow:
                            Follow();
                            break;*/

            case GiantState.Mount:
                animator.SetBool("move", false);
                Mount();
                break;

            case GiantState.Die:
                break;
        }

        if (notInRoom) return;

        // �н� ���ʹ� �Ʒ� ������ �������� �ʴ´�
        if (isCopyed) return;

        // �����ȿ� �÷��̾ �ְ�, ���� ���� �ʾҴٸ�
        if (IsPlayerInRange(range) && curState != GiantState.Die)
        {
            curState = GiantState.Mount;
        } 

        // �̹� �ѹ� ����Ʈ������ ���̻� wander���� �ʴ´�.
        else if (!IsPlayerInRange(range) && curState != GiantState.Die && !mounted)
        {
            curState = GiantState.Wander;
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
            curState = GiantState.Wander;

        if (IsPlayerInRange(range))
        {
            curState = GiantState.Mount;
        } 
    }

    // ���� ���� ����ٴ��� �����Ƿ� ����
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
            collision.gameObject.GetComponent<Player>().StartKnockBack(transform.position);
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
        if (mounted) return;

        mounted = true;

        Debug.Log("mapHeight" + mapHeight);

        SoundManager.instance.PlaySoundEffect("�𷡰��α渷");
        float startY = -mapHeight / 2 + enemyHeight / 2;

        GameObject closestCopyed = null;
        float closestDistance = float.MaxValue;

        // �н��� �����ϸ�, �н��� ���̰� ���� ��ü ���̸� �ʰ����� �ʵ��� ��       
        for (float y = startY; y <= mapHeight / 2; y += enemyHeight)
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            Vector3 spawnPosition = new Vector3(transform.position.x, y, 0);
            GameObject copyed = Instantiate(copyedGiantEnemy, spawnPosition, Quaternion.identity);
            copyed.GetComponent<SpriteRenderer>().flipX = transform.position.x < player.position.x;

            // ���� enemy�� �н� ���� �Ÿ� ���
            float distance = Vector3.Distance(transform.position, copyed.transform.position);

            // ���� ����� �н� ������Ʈ
            if (distance < closestDistance)
            {
                closestCopyed = copyed;
                closestDistance = distance;
            }
        }


        // ���� ����� �н��� ���� ����
        if (closestCopyed != null)
        {
            closestCopyed.GetComponent<GiantEnemy>().ranPotionDropPercent = 100;

            SpriteRenderer closestRenderer = closestCopyed.GetComponent<SpriteRenderer>();           
            Color newColor = closestRenderer.color;
            newColor.a = 1f; // ���ϴ� ������ ����
            closestRenderer.color = newColor;
        }
    }

    public void Death()
    {      
        // 25���� Ȯ���� �÷��̾� ü�� 1ȸ��
        int ran = Random.Range(0, 100);
 
        if (ran <= ranPotionDropPercent)
        {
            GameObject hpPotion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
            hpPotion.GetComponent<DropedPotion>().healHP = (int)(Random.Range(1, 8));    // 1~8���� ���� ü�� ȸ��
        }

        Instantiate(destoryAnimObj, transform.position, Quaternion.identity);   

        curState = GiantState.Die;
        Destroy(gameObject);

        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
