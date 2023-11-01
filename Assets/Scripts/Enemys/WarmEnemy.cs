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

    private int curHealth;          // 모래 거인 현재 체력

    [SerializeField]
    private int maxHealth = 3;          // 모래 거인 최대 체력

    [SerializeField]
    private float range;         // 플레이어를 인지하는 범위

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

        // 범위안에 플레이어가 있고, 현재 죽지 않았다면
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

        // 무작위 위치를 계산합니다.

        if (!coolDownAttack)
        {
            Vector3 randomOffset = Random.insideUnitSphere * radius;

            // 플레이어 주변 위치로 이동합니다.

            spawnPosition = playerTransform.position + randomOffset;

            // Poison 프리팹을 생성합니다.            
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

            // 잠시 보류
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
            SoundManager.instance.PlaySoundEffect("중간몹사망");
            Death();
        }
        else
        {
            SoundManager.instance.PlaySoundEffect("적피해입음");
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, playerTransform.position) <= range;
    }

    public void Death()
    {
        gameObject.SetActive(false);
        Destroy(this.gameObject, 4f);       // 독함정 로직이 돌아가야 하므로 조금 뒤에 삭제하도록 변경
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }

    void TransformToPoison()
    {
        if (warningObj != null)
        {
            // 경고 스프라이트를 파괴합니다.
            Destroy(warningObj);

            // 독 함정을 생성합니다.
            Instantiate(poisonPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
