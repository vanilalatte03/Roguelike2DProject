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
    public float maxHealthChange;
    public float moveSpeedChange;
    public float attackSpeedChange;
    public int bulletTypeChange;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    public void GetAbility()
    {
        GameController.instance.HealPlayer(healthChange);
        GameController.instance.MaxHealthChange(maxHealthChange);
        GameController.instance.MoveSpeedChange(moveSpeedChange);
        GameController.instance.FireRateChange(attackSpeedChange);
        GameController.instance.BulletTypeChange(bulletTypeChange);
        //GameController.instance.UpdateCollectedItems(this);
        //Destroy(gameObject);
    }
}