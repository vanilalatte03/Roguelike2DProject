using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FireState
{
    Idle,       // �⺻�� �����ϰ� �ִ� �����̴�.
    Die
}

public class FireEnemy : MonoBehaviour
{
    private Player player;
    private Transform playerTransform;
    private SpriteRenderer sprite;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Slider hpSlider;

    private int curHealth;          

    [SerializeField]
    private int maxHealth = 3;          

    [SerializeField]
    private float prevPlayerPosTime = 0.3f;

    [SerializeField]
    private float attackCoolTime = 1f;

    [SerializeField]
    private GameObject pillarPrefab;

    private GameObject pillarObj;
    private Vector3 playerLastPos;

    private FireState curState = FireState.Idle;
    private bool prevFounPlayerTime = false;
    private bool coolDownAttack = false;

 
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        player = GameController.instance.player;
        playerTransform = player.transform;
        playerLastPos = playerTransform.position;
        curHealth = maxHealth;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = curHealth;       
    }

    private void FixedUpdate()
    {
        switch (curState)
        {
            case FireState.Idle:
                Attack();
                break;
 
            case FireState.Die:
                break;

            default:
                break;
        }

        // �����ȿ� �÷��̾ �ְ�, ���� ���� �ʾҴٸ�
        /*if (IsPlayerInRange(range) && curState != FireState.Die)
        {
            curState = WarmState.Found;
        }

        else if (!IsPlayerInRange(range) && curState != WarmState.Die)
        {
            curState = WarmState.Idle;
        }*/
    }

    private void Attack()
    {
        if (playerTransform.position.x - transform.position.x < 0)
            sprite.flipX = false;
        else
            sprite.flipX = true;

        // ������ ��ġ�� ����մϴ�.


        if (!coolDownAttack)
        {
            StartCoroutine(CoolDown());
            if (!prevFounPlayerTime)
            {
                StartCoroutine(FounPlayerCool());
            }
        }     
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;

        yield return new WaitForSeconds(attackCoolTime);

        coolDownAttack = false;
    }

    private IEnumerator FounPlayerCool()
    {
        playerLastPos = playerTransform.position;
        prevFounPlayerTime = true;
        yield return new WaitForSeconds(prevPlayerPosTime);

        pillarObj = Instantiate(pillarPrefab, playerLastPos, Quaternion.identity);
        prevFounPlayerTime = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.DamagePlayer(1);

            // ��� ����
            // collision.GetComponent<Player>().StartKnockBack(transform.position);
        }
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



    public void Death()
    {
        gameObject.SetActive(false);
        Destroy(this.gameObject, 4f);       // ������ ������ ���ư��� �ϹǷ� ���� �ڿ� �����ϵ��� ����
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
