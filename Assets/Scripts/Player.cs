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
    public Vector2 resultVec;           //     ������ Vector2 moveVec; ���� ��ü

    [SerializeField]
    private SpriteRenderer bowSprite;

    private WaitForSeconds wait;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();      
    }

    // GameController�� Awake���� �̱��� �Ҵ����� �� ������ ����Ǹ� ������ ����Ƿ�, Awake�� �ƴ� Start���� ���ǵ带 �Ҵ��ؾ� ������ �ȳ�.
    private void Start()
    {
        speed = GameController.instance.MoveSpeed;
        wait = new WaitForSeconds(0.1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Heelo");
        }

        // ���� �ڵ�
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

        // ���̽�ƽ���� ���� �Է°���
        float joyX = joy.Horizontal;
        float joyY = joy.Vertical;

        // Ű����� ���� �Է°���
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

        // �ɸ��� �¿� ��ȯ
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

        // �������� resultVec��, ���̽�ƽ���� ������ ���� ���̽�ƽ ��������, Ű���� �Է��� ���� Ű���� �������� ������
        resultVec = (joyX != 0 || joyY != 0) ? new Vector2(joyX, joyY) : new Vector2(inputX, inputY); 
    }

    // ���� �������� Update�� �ƴ� FixedUpdate���� �ؾ� ������.
    // Update�� �������� ������ ���� �� ������ ȣ���.
    // FixedUpdate�� ������ �ð� �������� ����ǹǷ� ������ �̵� ������ ��������.
    private void FixedUpdate()
    {
        resultVec = resultVec.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + resultVec);
    }

    public void SetPlayerSpeed(float changedSpeed)
    {
        speed = changedSpeed;
    }

    // �ٸ� enemy ��ũ��Ʈ����, �÷��̾ �������� �� �˹� �ڷ�ƾ ȣ��
    public void StartKnockBack(Vector3 enemyPos)
    {
        StartCoroutine(KnockBack(enemyPos));  
    }

    private IEnumerator KnockBack(Vector3 enemyPos)
    {
        yield return wait;      // ���� �ϳ��� ���� �������� �������Ѵ�

        Vector3 dirVec = transform.position - enemyPos;

        transform.position = Vector3.Lerp(transform.position, dirVec, 5 * Time.deltaTime);
    }
}