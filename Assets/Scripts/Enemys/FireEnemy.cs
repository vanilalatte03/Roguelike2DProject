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

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private Slider hpSlider;

    private int curHealth;          

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private float prevPlayerPosTime;

    [SerializeField]
    private float attackCoolTime;

    private WaitForSeconds waitPrevPlayerPos;

    [SerializeField]
    private GameObject[] pillarPrefabs;

    [SerializeField]
    private GameObject paranetObj;

    private FireState curState = FireState.Idle;

    private bool isCoolDown;
    private bool isGeneratingFire;

    private bool initialWait = true;
    private float initialWaitTime = 3f;

    private GameObject fireObj;
    private bool isPowerUp = false;

    [SerializeField]
    private float powerUpHealth;

    [SerializeField]
    private float powerUpAttackCoolTime;

    private void Awake()
    {
        waitPrevPlayerPos = new WaitForSeconds(prevPlayerPosTime);
    }

    private void Start()
    {
        player = GameController.instance.player;
        playerTransform = player.transform;
        curHealth = maxHealth;
        hpSlider.maxValue = maxHealth;
        hpSlider.value = maxHealth;
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
            transform.localScale = new Vector3(4, 4, 1);

        else
            transform.localScale = new Vector3(-4, 4, 1);


        if (initialWait)
        {
            initialWaitTime -= Time.deltaTime;
            if (initialWaitTime <= 0)
            {
                initialWait = false;
            }
        } 
        
        else
        {  
            if (!isCoolDown && !isGeneratingFire)
            {
                StartCoroutine(SpwanFire());
                StartCoroutine(CoolDown());
            }
        } 
    }

    private IEnumerator SpwanFire()
    {
        isGeneratingFire = true;
        Vector3 playerLastPos = playerTransform.position;
        yield return waitPrevPlayerPos;

        int index = Random.Range(0, pillarPrefabs.Length);

        fireObj = Instantiate(pillarPrefabs[index], playerLastPos, Quaternion.identity);
        
        if (isPowerUp)
        {
            fireObj.GetComponent<FireAttack>().donDestroy = true;
        }

        SoundManager.instance.PlaySoundEffect("불공격");

        int ran = Random.Range(0, 2);
        
       /* if (ran == 0)
        {
            SoundManager.instance.PlaySoundEffect("불공격1");
        } else
        {
            SoundManager.instance.PlaySoundEffect("불공격2");
        }*/

        isGeneratingFire = false;     
    }

    private IEnumerator CoolDown()
    {
        isCoolDown = true;

        yield return new WaitForSeconds(isPowerUp ? powerUpAttackCoolTime : attackCoolTime);

        isCoolDown = false;
    }

/*
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

        int ranIndex = Random.Range(0, pillarPrefabs.Length);

        Instantiate(pillarPrefabs[ranIndex], playerLastPos, Quaternion.identity);
        prevFounPlayerTime = false;
    }   */


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.instance.DamagePlayer(1);

            // 잠시 보류
            GameController.instance.player.StartKnockBack(transform.position);
            collision.gameObject.GetComponent<Player>().StartKnockBack(transform.position);
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

        if (curHealth <= powerUpHealth && !isPowerUp)
        {
            isPowerUp = true;
            SoundManager.instance.PlaySoundEffect("파워업");
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
        paranetObj.SetActive(false);

        GameObject[] remainFiresObj = GameObject.FindGameObjectsWithTag("FireAttack");
    
        for (int i=0; i< remainFiresObj.Length; i++)
        {
            remainFiresObj[i].GetComponent<FireAttack>().FadeStart();
        }

        Destroy(paranetObj, 4f);       // 독함정 로직이 돌아가야 하므로 조금 뒤에 삭제하도록 변경
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());      
    }
}
