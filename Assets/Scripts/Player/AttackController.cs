using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireDelay;

    private float lastFire;

    public float bulletSize;
    


    // Update is called once per frame
    void Update()
    {

        // Defined "ShootHorizontal" and "ShootVertical" inside of player settings
        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVer = Input.GetAxis("ShootVertical");

        if ((shootHor != 0 || shootVer != 0) && Time.time > lastFire + fireDelay)
        {
            Shoot(shootHor, shootVer);
            lastFire = Time.time;
        }

    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0);

    }

    // the less our fireDelay the faster we shoot
    public void changeFireRate(float fireRateChange)
    {
        fireDelay -= fireRateChange;
    }

    public void changeBulletSize(float bulletSizeChange)
    {
        bulletSize += bulletSizeChange;
    }
}
