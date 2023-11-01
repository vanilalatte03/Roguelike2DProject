using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FireState
{
    Idle,       // 기본이 공격하고 있는 상태이다.
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
    private float attackCool = 3f;

    [SerializeField]
    private GameObject pillarPrefab;

    private GameObject prllarObj;

    private FireState curState = FireState.Idle;
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
            case FireState.Idle:
                Attack();
                break;
 
            case FireState.Die:
                break;

            default:
                break;
        }

        // 범위안에 플레이어가 있고, 현재 죽지 않았다면
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

        // 무작위 위치를 계산합니다.

        if (!coolDownAttack)
        {
            Vector3 randomOffset = Random.insideUnitSphere * radius;

            // 플레이어 주변 위치로 이동합니다.

            spawnPosition = playerTransform.position + randomOffset;

            prllarObj = Instantiate(pillarPrefab, spawnPosition, Quaternion.identity);

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



    public void Death()
    {
        gameObject.SetActive(false);
        Destroy(this.gameObject, 4f);       // 독함정 로직이 돌아가야 하므로 조금 뒤에 삭제하도록 변경
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
