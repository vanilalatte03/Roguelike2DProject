using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDamage : MonoBehaviour
{
    [SerializeField] private int EnermyDamage;

    [SerializeField] private HP _HP;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Damage();
        }
    }

    void Damage()
    {
        _HP.playerHealth = _HP.playerHealth - EnermyDamage;
        _HP.UpdateHealth();
        gameObject.SetActive(false);
    }
}
