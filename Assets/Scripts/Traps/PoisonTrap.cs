using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTrap : MonoBehaviour
{
    private float cooltime = 2f;
    private Coroutine trapCoroutine;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�ȳ�!");
        }
    }

    /* private void OnTriggerEnter2D(Collider2D collision)
     {
         if (!collision.CompareTag("Player")) return;

         trapCoroutine = StartCoroutine(trapCor());
     }

     private IEnumerator trapCor()
     {
         while(true)
         {
             GameController.instance.DamagePlayer(1);
             yield return new WaitForSeconds(5f);
         }
     }

     private void OnTriggerExit2D(Collider2D collision)
     {
         if (!collision.CompareTag("Player"))
             return;

         // �ڷ�ƾ ����
         if (trapCoroutine != null)
         {
             StopCoroutine(trapCoroutine);
             trapCoroutine = null; // �ڷ�ƾ ���� ����
         }
     }*/
}
