using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    public GameData gData;
    public List<GameObject> Lasers;
    public bool laserActive;
    public float laserTime;
    public float laserDelay;
    public laserClass laserData;

    private float count;

    private void OnEnable()
    {
        count = laserDelay;
        laserActive = false;
        foreach(GameObject laser in Lasers)
        {
            laser.GetComponent<LaserController>().data = laserData;
        }
    }

    void checkLaser()
    {
        count += Time.fixedDeltaTime;
        if (!gData.flying)
        {
            laserActive = false;
            foreach (GameObject laser in Lasers)
            {
                laser.SetActive(false);
            }
            gData.abilityText.SetActive(false);
            return;
        }
        if((count >= laserDelay) && !laserActive && Input.GetKey(KeyCode.Space))
        {
            count = 0;
            laserActive = true;
        }
        else if(laserActive && count >= laserTime)
        {
            count = 0;
            laserActive = false;
        }
        foreach (GameObject laser in Lasers)
        {
            laser.SetActive(laserActive);
        }
        if (gData.flying) { gData.abilityText.SetActive((count >= laserDelay) && !laserActive); }
    }

    private void FixedUpdate()
    {
        checkLaser();
    }
}
