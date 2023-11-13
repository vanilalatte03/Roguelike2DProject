using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTrap : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private int damageAmount; // �� ������ ���� ������ ��

    [SerializeField]
    public float damageInterval; // �������� ������ ���� (1.5�ʸ��� ������ ����)

    [SerializeField]
    private GameObject popEffect;

    [SerializeField]
    private float ranDistance;

    [SerializeField]
    private float secondSpawnDistance ;

    private GameObject firstEffectObj;
    private bool isDamaging = false; // ����� ������ ���θ� ��Ÿ���� �÷���    

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        animator.SetFloat("speed", -1f);
        SoundManager.instance.PlaySoundEffect("������");

        firstEffectObj = SpawnFirstEffect();
        SpawnObjectOpposite(firstEffectObj.transform.position);

        Invoke("FadeStart", 3f);
    }

    private GameObject SpawnFirstEffect()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-ranDistance, ranDistance), Random.Range(0, ranDistance * 1.2f), 0.0f);

        spawnPosition += transform.position;

        GameObject firstEffect = Instantiate(popEffect, spawnPosition, Quaternion.identity);

        return firstEffect;
    }

    void SpawnObjectOpposite(Vector3 firstObjPos)
    {
        // ��Ī ��ġ ���
        Vector3 symmetricalPosition = new Vector3(2 * firstObjPos.x - transform.position.x, firstObjPos.y, firstObjPos.z);

        // ��Ī ��ġ���� X �� ����
        symmetricalPosition.x += secondSpawnDistance;

        // ��Ī ��ġ���� Y �� ���� ����
        symmetricalPosition.y += Random.Range(-0.4f, 1.5f);

        // ������Ʈ ����
        Instantiate(popEffect, symmetricalPosition, Quaternion.identity);
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
