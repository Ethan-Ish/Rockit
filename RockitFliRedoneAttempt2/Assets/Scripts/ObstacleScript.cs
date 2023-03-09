using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{

    public obstacleClass obData;

    private void OnEnable()
    {
        if (gameObject.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D newRb = gameObject.AddComponent<Rigidbody2D>();
            newRb.gravityScale = obData.gameData.GetComponent<GameData>().Speed;
            newRb.gravityScale *= obData.gravScale;
            newRb.rotation = obData.rot;
            gameObject.GetComponent<Rigidbody2D>().angularVelocity = obData.angVel;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        if (collidedObj.tag == "Player")
        {
            obData.sfxMan.playSoundEffect(0);
            GameObject rocket = collidedObj;
            obData.gameData.GetComponent<GameData>().spawnObstacleParticle(gameObject.transform.position);
            obData.gameData.GetComponent<GameData>().rocketHealth -= 1;
            GameObject.Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.transform.position.y <= obData.yKillLvl)
        {
            GameObject.Destroy(gameObject);
        }
    }

}
