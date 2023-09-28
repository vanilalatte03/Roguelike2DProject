using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTrap : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 1; // �� ������ ���� ������ ��

    [SerializeField]
    public float damageInterval = 1.5f; // �������� ������ ���� (1.5�ʸ��� ������ ����)

    private bool isDamaging = false; // ����� ������ ���θ� ��Ÿ���� �÷���

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
            GameController.instance.DamagePlayer(damageAmount);           

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
}
