using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : MonoBehaviour
{   
    private SpriteRenderer spriteRenderer;

    [HideInInspector]
    public bool donDestroy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!donDestroy) FadeStart();
    }

    public void FadeStart()
    {
        StartCoroutine(Fade());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameController.instance.DamagePlayer(1);
        }
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(1f);

        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 2f;

            Color color = spriteRenderer.material.color;
            color.a = Mathf.Lerp(1, 0, percent);
            spriteRenderer.material.color = color;

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
