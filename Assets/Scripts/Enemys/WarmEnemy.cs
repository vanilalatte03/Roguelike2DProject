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
    public bool notInRoom;

    [SerializeField]
    private GameObject destoryAnimObj;

    [SerializeField]
    private Transform shadow;

    [SerializeField]
    private GameObject mouthEffectObj;

    [SerializeField]
    private GameObject potionPrefab;

    private Vector3 originShadowRot;
    private Vector3 flipShadowRot;

    private bool isLeft = true;

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

        originShadowRot = new Vector3(-0.05f, -0.42f, 20.45f);
        flipShadowRot = new Vector3(0.05f, -0.457f, -20.45f);
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
        if (notInRoom) return;

        if (playerTransform.position.x - transform.position.x < 0)
        {
            sprite.flipX = false;     
            shadow.rotation = Quaternion.Euler(originShadowRot);
            isLeft = true;
        }
       
        else
        {
            sprite.flipX = true;           
            shadow.rotation = Quaternion.Euler(flipShadowRot);
            isLeft = false;
        }        

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

            collision.gameObject.GetComponent<Player>().StartKnockBack(transform.position);
        }
    }

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

    void TransformToPoison()
    {
        if (warningObj != null)
        {
            // 경고 스프라이트를 파괴합니다.
            Destroy(warningObj);

            // 독 함정을 생성합니다.
            Instantiate(poisonPrefab, spawnPosition, Quaternion.identity);

            // 입김 이펙트
            GameObject mouthEffect = Instantiate(mouthEffectObj, isLeft ? new Vector3(transform.position.x - 2f, transform.position.y + 0.2f, 0) : new Vector3(transform.position.x + 2f, transform.position.y + 0.2f, 0), Quaternion.identity);

            if (isLeft)
            {
                mouthEffect.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }


    public void Death()
    {
        curState = WarmState.Die;
        gameObject.SetActive(false);

        // 25퍼의 확률로 플레이어 체력 1회복
        int ran = Random.Range(0, 100);

        if (ran <= 20)
        {
            GameObject hpPotion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
            hpPotion.GetComponent<DropedPotion>().healHP = 3;   // 3체력 회복
        }

        Instantiate(destoryAnimObj, transform.position, Quaternion.identity);    
        Destroy(this.gameObject, 4f);
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
