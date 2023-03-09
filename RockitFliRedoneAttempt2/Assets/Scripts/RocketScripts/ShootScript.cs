using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{

    public GameObject bullet;
    public GameData gData;
    public float shotDelay;
    public bulletClass defaultBulletData;
    public SfxManager sfxMan;

    private float counter;
    private bool canShoot;
    private defaultRocketFunc rFunc = new defaultRocketFunc();
    private customMath mathFunc = new customMath();
    private Vector3 startBulletPos;
    private Vector3 bulletDiff;

    private void OnEnable()
    {
        canShoot = true;
        counter = 0;
        gData.abilityText.SetActive(true);
    }

    private void Start()
    {
        startBulletPos = bullet.transform.position;
        bulletDiff = startBulletPos - gameObject.transform.position;
        bulletDiff.z = 1;
        bullet.SetActive(false);
    }

    public void spawnBullet()
    {
        sfxMan.playSoundEffect(2);
        GameObject newBullet = GameObject.Instantiate(bullet);
        newBullet.transform.position = gameObject.transform.position + bulletDiff;
        Vector2 newVel;
        newVel.x = 0;
        newVel.y = 0; // The bullet sets its own vertical velocity
        newBullet.GetComponent<Rigidbody2D>().velocity = newVel;
        newBullet.AddComponent<BulletScript>();
        newBullet.GetComponent<BulletScript>().data = defaultBulletData;
        newBullet.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (!canShoot) { counter += Time.fixedDeltaTime; }
        if (counter >= shotDelay)
        {
            counter = 0;
            canShoot = true;
        }
        if (gData.flying && Input.GetKey(KeyCode.Space) && canShoot)
        {
            spawnBullet();
            canShoot = false;
        }
        if(gData.flying) { gData.abilityText.SetActive(canShoot); }
    }

}
