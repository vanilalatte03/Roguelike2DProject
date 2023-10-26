using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTrap : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private int damageAmount = 1; // 독 함정이 입힐 데미지 양

    [SerializeField]
    public float damageInterval = 1.5f; // 데미지를 입히는 간격 (1.5초마다 데미지 입힘)

    [SerializeField]
    private float destroyTime = 3f;

    private bool isDamaging = false; // 대미지 중인지 여부를 나타내는 플래그

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
        // 충돌한 오브젝트가 플레이어인지 확인
        if (collision.CompareTag("Player"))
        {
            if (GameController.instance.Health <= 0)
            {
                StopCoroutine(DamagePlayer());
                return;
            }
           
            if (!isDamaging)
            {
                // 코루틴 시작: DamagePlayer 코루틴 실행
                StartCoroutine(DamagePlayer());
            }
        }
    }

    private IEnumerator DamagePlayer()
    {
        isDamaging = true; // 대미지 중임을 나타내는 플래그 설정

        while (isDamaging)
        {
            // 플레이어에게 데미지를 입힘
            GameController.instance.DamagePlayer(damageAmount, false);           

            // 일정 간격을 기다림
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 함정을 빠져나가면 대미지 중지
        if (collision.CompareTag("Player"))
        {
            isDamaging = false; // 대미지 중지
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
