using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Net;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject Sphinx;
    public static GameController instance;

    private float health = 6;
    private float maxHealth = 6;
    private float moveSpeed = 4f;
    private float fireRate = 0.5f;
    private int bulletCount = 0;
    private int power = 1;
    private int shield = 0;

    private bool bootCollected = false;
    private bool screwCollected = false;

    public List<string> collectedNames = new List<string>();

    public float Health { get => health; set => health = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public int BulletCount { get => bulletCount; set => bulletCount = value; }
    public int Power { get => power; set => power = value; }
    public int Shield { get => shield; set => shield = value; }

    [SerializeField]
    private TextMeshProUGUI healthText;
    // public Text healthText;
    public Player player;
    public CameraShake cameraShake;

    private float invincibilityTime = 2.0f; // ���� ���� �ð� ���� (��: 2��)

    private bool isInvincible = false;
    private float invincibleTimer = 0.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);      // �� ��ȯ�ÿ��� ����
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health + " / " + maxHealth;

        if (Input.GetKeyDown(KeyCode.T))
        {
            //���� �� �׽�Ʈ�� ���� ��ġ
            GameController.instance.BulletTypeChange(1);
            Debug.Log("Double Shot +1");
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
        
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
                player.ActionIsFadeStop();
            }
        } 
    }

    // �ι�° �Ű������� �÷��̾ ������ �޾��� �� ī�޶� ��鸲 ȿ���� ����, ����, �⺻�� true, ���� �ʰ� ������ false�� 
    public void DamagePlayer(int damage, bool shake = true)
    {
        if (isInvincible)
        {
            Debug.Log("���� �ð� �Դϴ�.");
            return;
        }

        isInvincible = true;
        invincibleTimer = invincibilityTime;
        player.FadePlayerStart();

        // ���� ���
        SoundManager.instance.PlaySoundEffect("�÷��̾����");

        //if (shield > 0)
        //    return;


        // ü�°���
        health -= damage;
        

        // ī�޶� ��鸲 ȿ��
        if (shake)
        {
            cameraShake.ShakeStart();       
        }       

        if (Health <= 0)
        {
            health = 0;
            KillPlayer();
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
        fireRate -= rate;
    }

    public void BulletTypeChange(int count)
    {
        bulletCount += count;
    }

    public void MaxHealthChange(float plus)
    {
        maxHealth += plus;
    }

    public void PowerChange(int pw)
    {
        power += pw;
    }

    public void ShieldChange(int sh)
    {
        shield += sh;
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
            FireRateChange(0.25f);
    }

    private void KillPlayer()
    {
        // Time.timeScale = 0f;
      
        EndSceneController.isClear = false;
        SceneManager.LoadScene("EndScene");             
    }
}
