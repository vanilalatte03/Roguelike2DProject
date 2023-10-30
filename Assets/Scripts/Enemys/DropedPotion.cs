using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropedPotion : MonoBehaviour
{
    [HideInInspector]
    public int healHP = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameController.instance.HealPlayer(healHP);
            SoundManager.instance.PlaySoundEffect("�����ü��ȸ��");
            Debug.Log(healHP + "�� ü���� ȸ���Ǿ����ϴ�!");
            Destroy(gameObject);
        }
    }
}
