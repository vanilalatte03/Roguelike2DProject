using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Item
{
    public string name;
    public string descripton;
    public Sprite itemImage;
}
public class CollectionController : MonoBehaviour
{

    public Item item;
    public float healthChange;
    

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameController.HealPlayer(healthChange); // ÈúÇÏ´Â ±¸¹®. 
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
