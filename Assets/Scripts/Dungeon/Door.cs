using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;

    public GameObject doorCollider;

    private GameObject player;
    public float widthOffset = 11.5f;
    public float heightOffset = 7f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SoundManager.instance.PlaySoundEffect("πÊ¿¸»Ø");

            switch (doorType)
            {
                case DoorType.bottom:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y - heightOffset);
                    break;
                case DoorType.left:
                    player.transform.position = new Vector2(transform.position.x - widthOffset, transform.position.y);
                    break;
                case DoorType.right:
                    player.transform.position = new Vector2(transform.position.x + widthOffset, transform.position.y);
                    break;
                case DoorType.top:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y + heightOffset - 3.0f);
                    break;
            }
        }
    }
}
