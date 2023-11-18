using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GuardianState
{
    Wander,     // ������ ���� (= Idle ����)
    Follow,     // �÷��̾ ������� ����
    Die         // ����
    // Attack�� OnTrigger�� ����
}

public class GuardianEnemy : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;

    [SerializeField]
    private GuardianState curState = GuardianState.Wander;

    private int curHealth;          // �� ���� ���� ü��

    [SerializeField]
    private int maxHealth;          // �� ���� �ִ� ü��

    [SerializeField]
    private float range;         // �÷��̾ �����ϴ� ����

    [SerializeField]
    private float speed;        // �̵� �ӵ�

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

        // �����ȿ� �÷��̾ �ְ�, ���� ���� �ʾҴٸ�
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

        speed = 0.7f;      // �̵� �ӵ� �ణ ������

        // Attack(); �ִϸ��̼� key event�� ��ü
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.DamagePlayer(1);

            collision.gameObject.GetComponent<Player>().StartKnockBack(transform.position);
        }
    }

    // ������ ���Ÿ� ����, �ִϸ��̼ǿ��� ȣ���� ����
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
        SoundManager.instance.PlaySoundEffect("��������");
    }

    // �ִϸ��̼ǿ��� ȣ���ϹǷ� �ʿ����
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
            SoundManager.instance.PlaySoundEffect("�߰������");
            Death();
        } else
        {
            SoundManager.instance.PlaySoundEffect("����������");
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
