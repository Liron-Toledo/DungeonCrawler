using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Passive,
    Agressive,
    Attacking,
    Dead
}

public enum EnemyType
{
    Melee,
    Ranged
}
public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyType enemytype;
    public EnemyState currentState = EnemyState.Idle;
    public float sightRange;
    private float speed;
    public float passiveSpeed;
    public float aggressiveSpeed;
    public float attackRange;
    private bool chooseDir = false;
    // private bool dead = false;
    private bool coolDownActive = false;
    public float coolDownLength;
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    // public bool playerInRoom = true;
    public bool notInRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (EnemyState.Idle):
                Idle();
                break;
            case (EnemyState.Passive):
                Passive();
                break;
            case (EnemyState.Agressive):
                Aggressive();
                break;
            case (EnemyState.Attacking):
                Attack();
                break;

        }

        if (!notInRoom)
        {
            if (isPlayerInRange(sightRange) && currentState != EnemyState.Dead)
            {
                currentState = EnemyState.Agressive;
            }
            else if (!isPlayerInRange(sightRange) && currentState != EnemyState.Dead)
            {
                currentState = EnemyState.Passive;
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currentState = EnemyState.Attacking;
            }
        }
        else
        {
            currentState = EnemyState.Idle;
        }

    }

    private bool isPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Room")
        {
            Debug.Log("Wall!");
        }
    }

    private IEnumerator ChooseDirection()
    {

        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f));
        // z-axis is used for rotation in 2D landscape
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        Quaternion nextRotation = Quaternion.Euler(randomDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
         
    }

    void Idle()
    {
        
    }

    void Passive()
    {
        speed = passiveSpeed;

        if(!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += transform.right * speed * Time.deltaTime;
        if(isPlayerInRange(sightRange))
        {
            currentState = EnemyState.Agressive;
        }
    }

    void Aggressive()
    {
        speed = aggressiveSpeed;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        if (!coolDownActive)
        {
            switch(enemytype)
            {
                case (EnemyType.Melee):
                    player.GetComponent<Health>().TakeDamage(1);
                    StartCoroutine(AttackCoolDown());
                    break;

                case (EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(AttackCoolDown());
                    break;
            }
        }
        
    }

    private IEnumerator AttackCoolDown()
    {
        coolDownActive = true;
        yield return new WaitForSeconds(coolDownLength);
        coolDownActive = false;
    }

    public void Death()
    {
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());
        Destroy(gameObject);
        currentState = EnemyState.Dead;
        // dead = true;
    }

}
