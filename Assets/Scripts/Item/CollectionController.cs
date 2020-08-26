using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public Sprite itemImage;
}

public class CollectionController : MonoBehaviour
{

    public Item item;
    public int healthChange;
    public float moveSpeedChange;
    public float firingSpeedChange;
    public float bulletSizeChange;

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<SpriteRenderer>().sprite = item.itemImage;

        // This is neccesary as colliders wont update if we change the sprite at runtime
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController.collectedAmount++;

            HealPlayer();
            IncreaseSpeed();
            IncreaseFireRate();
            IncreaseBulletSize();

            UpdateCollectedItmes();
            
            Destroy(gameObject);
        }
    }

    // Status Effects:

    void HealPlayer()
    {
        player.GetComponent<Health>().Heal(healthChange);
    }

    void IncreaseSpeed()
    {
        player.GetComponent<PlayerController>().ChangeSpeed(moveSpeedChange);
    }

    void IncreaseFireRate()
    {
        player.GetComponent<AttackController>().changeFireRate(firingSpeedChange);
    }

    void IncreaseBulletSize()
    {
        player.GetComponent<AttackController>().changeBulletSize(bulletSizeChange);
    }

    void UpdateCollectedItmes()
    {
        player.GetComponent<ItemController>().UpdateCollectedItems(this);
    }
}
