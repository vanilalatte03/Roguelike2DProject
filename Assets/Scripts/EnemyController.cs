using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Wander,     // 떠도는 상태
    Follow,     // 플레이어를 따라오는 상태
    Die,        // 죽은 상태
    Attack      // 공격
};

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currState = EnemyState.Wander;

    public float range;
    public float speed;
    private bool chooseDir = false;
    private bool coolDownAttack = false;
    private Vector3 randomDir;
    public float attackRange;
    public float coolDown;


    public GameObject enemySprite;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        switch (currState)
        {
            case EnemyState.Wander:
                Wander();
                break;
            case EnemyState.Follow:
                Follow();
                break;
            case EnemyState.Die:
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }

        if (isPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Follow;
        }
        else if(!isPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Wander;
        }
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            currState = EnemyState.Attack;
        }
    }

    private bool isPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    void ChooseDirection()
    {
        //yield return new WaitForSeconds(Random.Range(2f, 8f));
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }

    void Wander()
    {
        if(!chooseDir)
        {
            chooseDir = true;
            Invoke("ChooseDirection", Random.Range(2f, 8f));
        }

        transform.position += -transform.right * speed * Time.deltaTime;
        if(isPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        if (!coolDownAttack)
        {
            GameController.DamagePlayer(1);
            StartCoroutine(CoolDown());
        }
        
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    public void Death()
    {
        Destroy(enemySprite);
        Destroy(gameObject);
    }
}
