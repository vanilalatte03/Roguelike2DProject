using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [SerializeField] [Range(1f, 10f)] float barSpeed = 3f;
    public VariableJoystick joy;

    [SerializeField]
    private static float speed;

    SpriteRenderer rend;
    Rigidbody2D rb;

    [HideInInspector]
    public Vector2 resultVec;           //     ������ Vector2 moveVec; ���� ��ü

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();

        speed = GameController.instance.MoveSpeed;
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

        // �ɸ��� �¿� ��ȯ
        if (joyX > 0 || inputX > 0)
        {
            rend.flipX = false;
        }

        else if (joyX < 0 || inputX < 0)
        {
            rend.flipX = true;
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

    public static void SetPlayerSpeed(float changedSpeed)
    {
        Debug.Log("���� �÷��̾� ���ǵ� : " + speed);
        speed = changedSpeed;
        Debug.Log("���� �÷��̾� ���ǵ� "  + speed);
    }
}