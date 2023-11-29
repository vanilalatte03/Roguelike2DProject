using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // [SerializeField] [Range(1f, 10f)] float barSpeed = 3f;           사용하지 않으므로 일단 주석
    public VariableJoystick joy;

    private float speed;

    SpriteRenderer rend;
    Vector3 moveDir;
    Rigidbody2D rb;
    Animator animator;

    [HideInInspector]
    public Vector2 resultVec;                   // 기존의 Vector2 moveVec; 변수 대체

    [SerializeField]
    private SpriteRenderer bowSprite;

    private WaitForSeconds wait;
    private bool isKnockbacking = false;

    [SerializeField]
    private Transform runEffectPosLeft;

    [SerializeField]
    private Transform runEffectPosRight;

    [SerializeField]
    private GameObject runEffectObj;

    private float timer; // 이펙트 생성을 위한 타이머
    private bool isLeft;

    [HideInInspector]
    public bool isFadeStop;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();      
    }

    // GameController의 Awake에서 싱글톤 할당전에 이 로직이 실행되면 오류가 생기므로, Awake가 아닌 Start에서 스피드를 할당해야 오류가 안남.
    private void Start()
    {
        speed = GameController.instance.MoveSpeed;
        wait = new WaitForSeconds(0.1f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Heelo");
        }

        // 기존 코드
        /* float x = joy.Horizontal;
         float y = joy.Vertical;

         if (x >0)
         {
             rend.flipX = false;
         }

         else if (x <0)
         {
             rend.flipX = true;
         }

         moveVec = new Vector2(x, y) * speed * Time.deltaTime;
         rb.MovePosition(rb.position + moveVec);*/

        // 조이스틱으로 받은 입력값들
        float joyX = joy.Horizontal;
        float joyY = joy.Vertical;  

        // 키보드로 받은 입력값들
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0 || inputY != 0 ||
            joyX != 0 || joyY != 0)
        {
            animator.SetBool("Idle", false);
            CreateWalkEffect();     // 이펙트 생성
        }

        else
        {
            animator.SetBool("Idle", true);
        }

        // 케릭터 좌우 전환
        if (joyX > 0 || inputX > 0)
        {
            rend.flipX = false;
            bowSprite.flipX = false;
            isLeft = false;
        }

        else if (joyX < 0 || inputX < 0)
        {
            rend.flipX = true;
            bowSprite.flipX = true;
            isLeft = true;
        }

        // 전역변수 resultVec에, 조이스틱으로 움직일 때는 조이스틱 방향으로, 키보드 입력일 때는 키보드 방향으로 움직임
        resultVec = (joyX != 0 || joyY != 0) ? new Vector2(joyX, joyY) : new Vector2(inputX, inputY);
    }

    private void CreateWalkEffect()
    {
        if (timer >= 1.2f)
        {
            Instantiate(runEffectObj, isLeft ? runEffectPosLeft.position : runEffectPosRight.position, Quaternion.identity);
            timer = 0;
        }       
    }

    // 물리 움직임은 Update가 아닌 FixedUpdate에서 해야 안정적.
    // Update는 프레임이 높아질 수록 더 빠르게 호출됨.
    // FixedUpdate는 고정된 시간 간격으로 실행되므로 물리적 이동 로직에 안정적임.
    private void FixedUpdate()
    {
        if (isKnockbacking) return;

        resultVec = resultVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + resultVec);
    }

    public void SetPlayerSpeed(float changedSpeed)
    {
        speed = changedSpeed;
    }

    // 다른 enemy 스크립트에서, 플레이어를 공격했을 때 넉백 코루틴 호출
    public void StartKnockBack(Vector3 enemyPos)
    {
        StartCoroutine(KnockBack(enemyPos));  
    }

    private IEnumerator KnockBack(Vector3 enemyPos)
    {
        isKnockbacking = true;
        yield return wait;      // 다음 하나의 물리 프레임을 딜레이한다

        Debug.Log("플레이어 넉백");

        Vector3 dirVec = transform.position - enemyPos;
        dirVec.Normalize(); // 방향 벡터 정규화

        // Debug.DrawLine(transform.position, transform.position + dirVec, Color.red, 5f);
        rb.AddForce(dirVec * 1.5f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        isKnockbacking = false;
    }

    public void FadePlayerStart()
    {
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0.2f));
            yield return StartCoroutine(Fade(0.2f, 1));

            if (isFadeStop) 
            {
                isFadeStop = false;
                break;
            }
        }
    }

    public void ActionIsFadeStop()
    {
        if (!isFadeStop)
        {
            isFadeStop = true;
        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float curTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1f)
        {
            curTime += Time.deltaTime;
            percent = curTime / 0.2f;

            Color color = rend.color;
            color.a = Mathf.Lerp(start, end, percent);
            rend.color = color;

            yield return null;
        }
    }
}