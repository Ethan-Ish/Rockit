using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseScript : MonoBehaviour
{

    public GameObject rocket;
    public GameData gData;
    public bool phased;
    public float phaseLength;
    public float phaseDelay;
    public float phasePulseConstant;
    public float alpha1;
    public float alpha2;

    private float count;

    private void OnEnable()
    {
        count = phaseDelay;
        phased = false;
        rocket.tag = "Player";
        gData.abilityText.SetActive(true);
    }

    public void checkPhase()
    {
        if(!gData.flying)
        {
            phased = false;
            count = 0;
            return;
        }
        count += Time.fixedDeltaTime;
        if(phased)
        {
            if(count >= phaseLength)
            {
                count = 0;
                phased = false;
            }
        }
        else if((count >= phaseDelay) && Input.GetKey(KeyCode.Space))
        {
            count = 0;
            phased = true;
        }
        if (gData.flying) { gData.abilityText.SetActive((count >= phaseDelay) && !phased); }
    }

    public bool phaseState(float input, float constant)
    {
        float sinVal = Mathf.Sin(input * constant);
        bool returnBool = (sinVal > 0);
        return returnBool;
    }

    public float increaseSpeedConstant(float input)
    {
        float diff = phaseLength - count;
        float returnFloat = 1 / diff; // Inverts (smaller diff = larger number)
        returnFloat *= 10;
        float max = 50;
        if(returnFloat >= max) { returnFloat = max; }
        if(diff < 1) { returnFloat = max; } // Fractions mess up the inversion!
        return returnFloat;
    }

    public void reactToPhase()
    {
        if(phased)
        {
            rocket.tag = "PhasedPlayer";
            rocket.layer = 11;
            float currentConstant = increaseSpeedConstant(phasePulseConstant);
            if (phaseState(Time.time, currentConstant))
            {
                Color newColor;
                newColor = new Color(1, 1, 1);
                newColor.a = alpha1;
                rocket.GetComponent<SpriteRenderer>().color = newColor;
            }
            else
            {
                Color newColor;
                newColor = new Color(1, 1, 1);
                newColor.a = alpha2;
                rocket.GetComponent<SpriteRenderer>().color = newColor;
            }
        }
        else
        {
            rocket.tag = "Player";
            rocket.layer = 9;
            Color newColor;
            newColor = new Color(1, 1, 1, 1); //Makes it opaque
            rocket.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    private void FixedUpdate()
    {
        checkPhase();
        reactToPhase();
    }

}
