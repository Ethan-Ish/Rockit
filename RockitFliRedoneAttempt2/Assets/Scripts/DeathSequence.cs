using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSequence : MonoBehaviour
{

    public SfxManager sfxMan;
    public GameObject explosionParticle;
    public GameObject particleManager;
    public GameObject deathScreen1;
    public GameObject deathScreen2;
    public GameObject menuManager;
    public CameraLerp camScript;
    public Vector3 corner1;
    public Vector3 corner2;
    public int minExplosionCount;
    public int maxExplosionCount;
    public GameData gameData;
    public List<GameObject> whiteParticles;
    public List<GameObject> redParticles;
    
    private positionFunc posFunc = new positionFunc();
    private customMath mathFunc = new customMath();

    public void deathSequence()
    {
        StartCoroutine(dSeq());
    }
    
    public void createExplosion()
    {
        Vector3 partPos = posFunc.getPosBetweenCorners(corner1, corner2);
        ParticleSystem partSys = explosionParticle.GetComponent<ParticleSystem>();
        ParticleGenerator particleGen = particleManager.GetComponent<ParticleGenerator>();
        particleGen.spawnParticles(partSys, partPos);
    }

    public void activateAllParticles(List<GameObject> gameObjList)
    {
        ParticleGenerator particleGen = particleManager.GetComponent<ParticleGenerator>();
        foreach(GameObject partSysObj in gameObjList)
        {
            Vector3 partPos = partSysObj.transform.position;
            ParticleSystem partSys = partSysObj.GetComponent<ParticleSystem>();
            particleGen.spawnParticles(partSys, partPos);
        }
    }

    public float waitTimeFormula(float input)
    {
        float returnFloat;
        returnFloat = 6;
        returnFloat *= Mathf.Pow((input + 15), -1f);
        returnFloat -= 0.2f;
        if(returnFloat < 0) { returnFloat = 0; }
        return returnFloat;
    }

    public IEnumerator dSeq()
    {
        GameData gData = gameData.GetComponent<GameData>();
        MenuManager menuMan = menuManager.GetComponent<MenuManager>();
        ObstacleManager obsManager = gameData.obsManager.GetComponent<ObstacleManager>();
        obsManager.deleteAllObstacles();
        obsManager.spawn = false;
        gData.abilityText.SetActive(false);
        int explosionCount = Random.Range(minExplosionCount, maxExplosionCount);
        for (int i = 0; i < explosionCount; i++)
        {
            createExplosion();
            yield return new WaitForSeconds(Random.Range(0.001f,0.030f));
        }
        yield return new WaitForSeconds(0.5f);
        sfxMan.playSoundEffect(0);
        activateAllParticles(whiteParticles);
        deathScreen1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        sfxMan.playSoundEffect(0);
        activateAllParticles(redParticles);
        deathScreen2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        while(gData.meters > 0)
        {
            sfxMan.playSoundEffect(1);
            gData.meters -= 1;
            menuMan.Money += 1;
            yield return new WaitForSeconds(waitTimeFormula(gData.meters));
        }
        yield return new WaitForSeconds(1);
        camScript.moveCamera(0);
        GameObject activeRocket = menuMan.currentRocketData.rocketPlayer;
        activeRocket.SetActive(false);
        activeRocket.transform.localPosition = new Vector3(0, 0, -2f); // recenters the rocket
    }
    
}
