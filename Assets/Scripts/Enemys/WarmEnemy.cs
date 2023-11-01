using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WarmState {
    Idle,
    Found,
    Die
}

public class WarmEnemy : MonoBehaviour
{
    private Player player;
    private Transform playerTransform;
    private SpriteRenderer sprite;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Slider hpSlider;

    private int curHealth;          // �� ���� ���� ü��

    [SerializeField]
    private int maxHealth = 3;          // �� ���� �ִ� ü��

    [SerializeField]
    private float range;         // �÷��̾ �����ϴ� ����

    [SerializeField]
    private float attackCool = 3f;

    [SerializeField]
    private GameObject poisonPrefab;

    [SerializeField]
    private GameObject warnningPrefab;

    private GameObject warningObj;

    private WarmState curState = WarmState.Idle;
    private bool coolDownAttack = false;
    private Vector3 spawnPosition;

    [SerializeField]
    private float radius;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        player = GameController.instance.player;
        playerTransform = player.transform;
        curHealth = maxHealth;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = curHealth;
    }

    private void FixedUpdate()
    {
        switch (curState)
        {
            case WarmState.Idle:
                break;
            case WarmState.Found:
                Attack();
                break;
            case WarmState.Die:
                break;

            default:
                break;
        }

        // �����ȿ� �÷��̾ �ְ�, ���� ���� �ʾҴٸ�
        if (IsPlayerInRange(range) && curState != WarmState.Die)
        {
            curState = WarmState.Found;
        }

        else if (!IsPlayerInRange(range) && curState != WarmState.Die)
        {
            curState = WarmState.Idle;
        }
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
            Vector3 randomOffset = Random.insideUnitSphere * radius;

            // �÷��̾� �ֺ� ��ġ�� �̵��մϴ�.

            spawnPosition = playerTransform.position + randomOffset;

            // Poison �������� �����մϴ�.            
            warningObj = Instantiate(warnningPrefab, spawnPosition, Quaternion.identity);

            Invoke("TransformToPoison", 1.0f);

            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(attackCool);
        coolDownAttack = false;
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

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= range;
    }

    public void Death()
    {
        gameObject.SetActive(false);
        Destroy(this.gameObject, 4f);       // ������ ������ ���ư��� �ϹǷ� ���� �ڿ� �����ϵ��� ����
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }

    void TransformToPoison()
    {
        if (warningObj != null)
        {
            // ��� ��������Ʈ�� �ı��մϴ�.
            Destroy(warningObj);

            // �� ������ �����մϴ�.
            Instantiate(poisonPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
