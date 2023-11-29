using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphinxEnemy : MonoBehaviour
{
    Animator animator;
    GameObject player;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Slider hpSlider;
    public int maxHealth;
    public int health;
    private float coolDown;
    private bool coolDownAttack = false;
    public bool notInRoom = false;
    public GameObject bulletPrefab;

    [SerializeField]
    private GameObject destoryAnimObj;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        hpSlider.maxValue = maxHealth;
        health = maxHealth;
        hpSlider.value = health;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MeleeAttack();
        }
    }

    public void MeleeAttack()
    {
        if (!coolDownAttack)
        {
            GameController.instance.DamagePlayer(1);
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    public void Damaged(int dm)
    {
        health -= dm;
        hpSlider.value = health;

        if (!canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(true);
        }

        if (health <= 0)
        {
            SoundManager.instance.PlaySoundEffect("Áß°£¸÷»ç¸Á");
            Death();
        }
        else
        {
            SoundManager.instance.PlaySoundEffect("ÀûÇÇÇØÀÔÀ½");
        }
    }

    public void Death()
    {
        // RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine()); 
        Instantiate(destoryAnimObj, transform.position, Quaternion.identity);        // ¾ê´Â »ç¸Á ÀÌÆåÆ®
        Destroy(gameObject);
        Invoke("GoToStage2", 2);
    }

    private void GoToStage2()
    {
        LoadingSceneController.LoadScene("StoneMain");
    }
}
