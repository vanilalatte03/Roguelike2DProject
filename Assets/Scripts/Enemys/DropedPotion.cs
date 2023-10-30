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
            SoundManager.instance.PlaySoundEffect("드랍된체력회복");
            Debug.Log(healHP + "의 체력이 회복되었습니다!");
            Destroy(gameObject);
        }
    }
}
