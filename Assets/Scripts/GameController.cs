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

    private float invincibilityTime = 2.0f; // 무적 지속 시간 설정 (예: 2초)

    private bool isInvincible = false;
    private float invincibleTimer = 0.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);      // 씬 전환시에도 유지
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health + " / " + maxHealth;

        if (Input.GetKeyDown(KeyCode.T))
        {
            //더블 샷 테스트를 위한 장치
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

    // 두번째 매개변수는 플레이어가 데미지 받았을 때 카메라 흔들림 효과를 할지, 말지, 기본은 true, 받지 않고 싶으면 false로 
    public void DamagePlayer(int damage, bool shake = true)
    {
        if (isInvincible)
        {
            Debug.Log("무적 시간 입니다.");
            return;
        }

        isInvincible = true;
        invincibleTimer = invincibilityTime;
        player.FadePlayerStart();

        // 사운드 출력
        SoundManager.instance.PlaySoundEffect("플레이어데미지");

        //if (shield > 0)
        //    return;


        // 체력감소
        health -= damage;
        

        // 카메라 흔들림 효과
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
