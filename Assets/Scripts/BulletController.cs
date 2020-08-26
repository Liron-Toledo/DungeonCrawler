using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;

    public GameObject player;

    public bool isEnemyBullet = false;

    private Vector2 lastPos;
    private Vector2 currentPos;
    private Vector2 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathDelay());
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (!isEnemyBullet)
        {
            transform.localScale = new Vector2(player.GetComponent<AttackController>().bulletSize, player.GetComponent<AttackController>().bulletSize);
        }
      
    }

    void Update()
    {
        if (isEnemyBullet)
        {
            currentPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            if (currentPos == lastPos)
            {
                Destroy(gameObject);
            }
            lastPos = currentPos;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !isEnemyBullet)
        {
            collision.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        if(collision.tag == "Player" && isEnemyBullet)
        {
            player.GetComponent<Health>().TakeDamage(1);
            Destroy(gameObject);
        }
    }

}
