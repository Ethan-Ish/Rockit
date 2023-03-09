using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameData : MonoBehaviour
{

    [Header("Rocket Data")]

    public float Speed;
    public float rocketHealth;
    public float rocketMaxHealth;
    public bool flying;
    public int meters;

    [Header("Objects to Manipulate")]

    public GameObject starsGame;
    public GameObject abilityText;
    public GameObject starsParallax;
    public GameObject deathSequence;
    public Image healthBarMeter;
    public GameObject rocketFolder;
    public GameObject particle;
    public GameObject obsManager;
    public ParticleSystem obsParticles;
    public float count;

    private defaultRocketFunc rFunc = new defaultRocketFunc();

    public void spawnObstacleParticle(Vector3 pos)
    {
        particle.GetComponent<ParticleGenerator>().spawnParticles(obsParticles, pos);
    }

    public void deactivateAllRockets()
    {
        for (int i = 0; i < rocketFolder.transform.childCount; ++i)
        {
            Transform rocketTrans = gameObject.transform.GetChild(i);
            GameObject rocketGameObj = rocketTrans.gameObject;
            rocketGameObj.SetActive(false);
        }
    }

    public float distanceFormula(float input)
    {
        float returnFloat;
        returnFloat = 2;
        returnFloat /= (input + 4);
        returnFloat += 0.5f;
        return returnFloat;
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        starsGame.GetComponent<StarMove>().speed = Speed;
        starsParallax.GetComponent<StarMove>().speed = Speed * 0.5f;
        rFunc.updateBar(rocketMaxHealth, rocketHealth, healthBarMeter, 12.4f);
    }

    private void Update()
    {
        if(flying)
        {
            count += Time.deltaTime;
            if(count >= distanceFormula(Speed))
            {
                count = 0;
                meters += 1;
            }
        }
    }

}