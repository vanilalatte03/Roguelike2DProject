using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    [SerializeField] [Range(1f, 10f)] float barSpeed = 3f;
    public VariableJoystick joy;
    public float speed;
    SpriteRenderer rend;

    Rigidbody2D rb;
    Vector2 moveVec;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Heelo");
        }

        float x = joy.Horizontal;
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
        rb.MovePosition(rb.position + moveVec);
    }
}