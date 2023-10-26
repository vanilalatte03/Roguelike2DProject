using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTrap : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private int damageAmount = 1; // �� ������ ���� ������ ��

    [SerializeField]
    public float damageInterval = 1.5f; // �������� ������ ���� (1.5�ʸ��� ������ ����)

    [SerializeField]
    private float destroyTime = 3f;

    private bool isDamaging = false; // ����� ������ ���θ� ��Ÿ���� �÷���

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        animator.SetFloat("speed", -1f);
        Invoke("FadeStart", 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �浹�� ������Ʈ�� �÷��̾����� Ȯ��
        if (collision.CompareTag("Player"))
        {
            if (GameController.instance.Health <= 0)
            {
                StopCoroutine(DamagePlayer());
                return;
            }
           
            if (!isDamaging)
            {
                // �ڷ�ƾ ����: DamagePlayer �ڷ�ƾ ����
                StartCoroutine(DamagePlayer());
            }
        }
    }

    private IEnumerator DamagePlayer()
    {
        isDamaging = true; // ����� ������ ��Ÿ���� �÷��� ����

        while (isDamaging)
        {
            // �÷��̾�� �������� ����
            GameController.instance.DamagePlayer(damageAmount, false);           

            // ���� ������ ��ٸ�
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �÷��̾ ������ ���������� ����� ����
        if (collision.CompareTag("Player"))
        {
            isDamaging = false; // ����� ����
        }
    }

    private void FadeStart()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 2f;

            Color color = spriteRenderer.material.color;
            color.a = Mathf.Lerp(1, 0, percent);
            spriteRenderer.material.color = color;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
