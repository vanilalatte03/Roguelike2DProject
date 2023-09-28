using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private float health = 6;
    private float maxHealth = 6;
    private float moveSpeed = 4f;
    private float fireRate = 0.5f;
    private float bulletCount = 0.5f;

    private bool bootCollected = false;
    private bool screwCollected = false;

    public List<string> collectedNames = new List<string>();

    public float Health { get => health; set => health = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float BulletCount { get => bulletCount; set => bulletCount = value; }

    public Text healthText;
    public Player player;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health + " / " + maxHealth;
    }

    public void DamagePlayer(int damage)
    {
        health -= damage;

        if (Health <= 0)
        {
            health = 0;
           // 잠시 주석 KillPlayer();
        }
    }

    public void HealPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount);
    }

    public void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
        player.SetPlayerSpeed(moveSpeed);
    }

    public void FireRateChange(float rate)
    {
        Debug.Log("이전 공속" + fireRate);
        fireRate -= rate;
        Debug.Log("이후 공속" + fireRate);
    }

    public void BulletTypeChange(int count)
    {
        bulletCount += count;
    }

    public void MaxHealthChange(float plus)
    {
        maxHealth += plus;
    }

    public void UpdateCollectedItems(CollectionController item)
    {
        collectedNames.Add(item.item.name);

        foreach (string i in collectedNames)
        {
            switch (i)
            {
                case "Boot":
                    bootCollected = true;
                    break;
                case "Screw":
                    screwCollected = true;
                    break;
            }
        }

        if (bootCollected && screwCollected)
        {
            FireRateChange(0.25f);
        }
    }

    private void KillPlayer()
    {
        Time.timeScale = 0f;
        GameOverUI.ShowGameOverBack();
    }
}
