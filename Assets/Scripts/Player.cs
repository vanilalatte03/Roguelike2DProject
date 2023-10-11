using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [SerializeField] [Range(1f, 10f)] float barSpeed = 3f;
    public VariableJoystick joy;

    private float speed;

    SpriteRenderer rend;
    Vector3 moveDir;
    Rigidbody2D rb;
    Animator animator;

    [HideInInspector]
    public Vector2 resultVec;           //     기존의 Vector2 moveVec; 변수 대체

    [SerializeField]
    private SpriteRenderer bowSprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();

        speed = GameController.instance.MoveSpeed;
    }

    private void Update()
    {
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
        }

        else if (joyX < 0 || inputX < 0)
        {
            rend.flipX = true;
            bowSprite.flipX = true;
        }

        // 전역변수 resultVec에, 조이스틱으로 움직일 때는 조이스틱 방향으로, 키보드 입력일 때는 키보드 방향으로 움직임
        resultVec = (joyX != 0 || joyY != 0) ? new Vector2(joyX, joyY) : new Vector2(inputX, inputY); 
    }

    // 물리 움직임은 Update가 아닌 FixedUpdate에서 해야 안정적.
    // Update는 프레임이 높아질 수록 더 빠르게 호출됨.
    // FixedUpdate는 고정된 시간 간격으로 실행되므로 물리적 이동 로직에 안정적임.
    private void FixedUpdate()
    {
        resultVec = resultVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + resultVec);
    }

    public void SetPlayerSpeed(float changedSpeed)
    {
        Debug.Log("이전 플레이어 스피드 : " + speed);
        speed = changedSpeed;
        Debug.Log("현재 플레이어 스피드 "  + speed);
    }
}