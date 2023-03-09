using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    public laserClass data;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObj = collision.gameObject;
        if(collidedObj.tag == "Obstacle")
        {
            Vector3 pos = collision.GetContact(0).point;
            pos.z = data.redParticles.transform.position.z;
            ParticleSystem pSys = data.redParticles.GetComponent<ParticleSystem>();
            data.pGen.spawnParticles(pSys, pos);
            GameObject.Destroy(collidedObj);
            data.gData.rocketHealth += 1;
        }
    }

    private void Update()
    {
        float newX = Mathf.Abs(Mathf.Sin(data.speedConstant * Time.time));
        newX += data.minimum;
        newX *= data.constant; // y = b * ( |sin(ax)| + c)
        Vector3 newScale = gameObject.transform.localScale;
        newScale.x = newX;
        gameObject.transform.localScale = newScale;
    }

}
