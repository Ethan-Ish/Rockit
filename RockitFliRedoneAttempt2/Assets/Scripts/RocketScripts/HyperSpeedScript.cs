using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperSpeedScript : MonoBehaviour
{

    public GameObject rocket;
    public GameData gData;
    public float coolDown;
    public float hyperLength;
    public bool hyper;
    public float normalSpeedIncrease;
    public float addSpeed;
    public GameObject hyperFlame;

    private float count;
    private float returnSpeed;

    private void OnEnable()
    {
        returnSpeed = rocket.GetComponent<DefaultRocketScript>().rPD.defaultSpeed;
        voidHyper();
        count = coolDown;
        gData.abilityText.SetActive(true);
    }

    public void voidHyper()
    {
        count = 0;
        hyper = false;
        rocket.GetComponent<DefaultRocketScript>().rPD.speed = returnSpeed;
        rocket.GetComponent<DefaultRocketScript>().rPD.speedIncreaseValue = normalSpeedIncrease;
    }

    public void startHyper()
    {
        count = 0;
        hyper = true;
        returnSpeed = rocket.GetComponent<DefaultRocketScript>().rPD.speed;
        rocket.GetComponent<DefaultRocketScript>().rPD.speed = returnSpeed + addSpeed;
        rocket.GetComponent<DefaultRocketScript>().rPD.speedIncreaseValue = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obstacle = collision.gameObject;
        if((obstacle.tag == "Obstacle") && hyper)
        {
            gData.rocketHealth += 1;
        }
    }

    public void checkHyper()
    {
        if (!gData.flying)
        {
            voidHyper();
        }
        count += Time.fixedDeltaTime;
        if(hyper)
        {
            if(count >= hyperLength)
            {
                voidHyper();
            }
        }
        else if((count >= coolDown) && Input.GetKey(KeyCode.Space))
        {
            startHyper();
        }
        hyperFlame.SetActive(hyper);
        if (gData.flying) { gData.abilityText.SetActive((count >= coolDown) && !hyper); }
    }

    private void FixedUpdate()
    {
        checkHyper();
    }

}
