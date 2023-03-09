using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    public void spawnParticles(ParticleSystem particle, Vector3 particlePos)
    {
        GameObject pSysObj = particle.gameObject;
        GameObject newPSysObj = GameObject.Instantiate(pSysObj);
        newPSysObj.transform.parent = pSysObj.transform.parent;
        newPSysObj.transform.position = particlePos;
        newPSysObj.SetActive(true);
        StartCoroutine(playParticleThenDestroy(newPSysObj.GetComponent<ParticleSystem>()));
    }

    public IEnumerator playParticleThenDestroy(ParticleSystem pSys)
    {
        float waitTime = pSys.main.duration;
        pSys.Play();
        yield return new WaitForSeconds(waitTime);
        GameObject pSysObj = pSys.gameObject;
        GameObject.Destroy(pSysObj);
    }

}