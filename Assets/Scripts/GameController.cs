using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private static float health = 6;
    private static int maxHealth = 6;

    public static float Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }

    public Text healthText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if (Health <= 0)
        {
            KillPlayer();
        }
    }

    public static void HealPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount);
    }

    private static void KillPlayer()
    {

    }
}
