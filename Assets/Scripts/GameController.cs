using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject Sphinx;
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
    public CameraShake cameraShake;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);      // 씬 전환시에도 유지
        } 
        
        else
        {
            Destroy(gameObject);                // 이미 존재하는 인스턴스 파괴
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health + " / " + maxHealth;
    }

    // 두번째 매개변수는 플레이어가 데미지 받았을 때 카메라 흔들림 효과를 할지, 말지, 기본은 true, 받지 않고 싶으면 false로 
    public void DamagePlayer(int damage, bool shake = true)
    {
        // 사운드 출력
        SoundManager.instance.PlaySoundEffect("플레이어데미지");

        // 체력감소
        health -= damage;

        // 카메라 흔들림 효과
        if (shake)
        {
            // cameraShake.ShakeStart();        // 카메라 흔들림 효과 생략 
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
        
        // 죽으면 이동하는 씬, 잠시 보류
        // SceneManager.LoadScene("EndScene");
    }
}
