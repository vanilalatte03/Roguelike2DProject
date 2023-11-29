using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // [SerializeField] [Range(1f, 10f)] float barSpeed = 3f;           ������� �����Ƿ� �ϴ� �ּ�
    public VariableJoystick joy;

    private float speed;

    SpriteRenderer rend;
    Vector3 moveDir;
    Rigidbody2D rb;
    Animator animator;

    [HideInInspector]
    public Vector2 resultVec;                   // ������ Vector2 moveVec; ���� ��ü

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

    private float timer; // ����Ʈ ������ ���� Ÿ�̸�
    private bool isLeft;

    [HideInInspector]
    public bool isFadeStop;

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
        timer += Time.deltaTime;
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
            CreateWalkEffect();     // ����Ʈ ����
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
            isLeft = false;
        }

        else if (joyX < 0 || inputX < 0)
        {
            rend.flipX = true;
            bowSprite.flipX = true;
            isLeft = true;
        }

        // �������� resultVec��, ���̽�ƽ���� ������ ���� ���̽�ƽ ��������, Ű���� �Է��� ���� Ű���� �������� ������
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

    // ���� �������� Update�� �ƴ� FixedUpdate���� �ؾ� ������.
    // Update�� �������� ������ ���� �� ������ ȣ���.
    // FixedUpdate�� ������ �ð� �������� ����ǹǷ� ������ �̵� ������ ��������.
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

    // �ٸ� enemy ��ũ��Ʈ����, �÷��̾ �������� �� �˹� �ڷ�ƾ ȣ��
    public void StartKnockBack(Vector3 enemyPos)
    {
        StartCoroutine(KnockBack(enemyPos));  
    }

    private IEnumerator KnockBack(Vector3 enemyPos)
    {
        isKnockbacking = true;
        yield return wait;      // ���� �ϳ��� ���� �������� �������Ѵ�

        Debug.Log("�÷��̾� �˹�");

        Vector3 dirVec = transform.position - enemyPos;
        dirVec.Normalize(); // ���� ���� ����ȭ

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