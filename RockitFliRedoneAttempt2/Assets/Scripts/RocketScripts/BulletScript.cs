using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public bulletClass data;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        ParticleGenerator pGen = data.pGen;
        if(collidedObj.tag == "Obstacle")
        {
            data.sfxMan.playSoundEffect(0);
            GameObject.Destroy(collidedObj);
            data.menuMan.Money += data.sellPrice;
            ParticleSystem pSys = data.particles.GetComponent<ParticleSystem>();
            Vector3 newPos = gameObject.transform.position;
            newPos.z = -4f;
            pGen.spawnParticles(pSys, newPos);
            GameObject.Destroy(gameObject);
        }
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if(gameObject.transform.position.y < -20 || gameObject.transform.position.y > 20)
        {
            GameObject.Destroy(gameObject);
        }
        Vector2 newVel = new Vector2(0,0);
        newVel.y = data.verticalVelocity;
        gameObject.GetComponent<Rigidbody2D>().velocity = newVel;
    }
}