using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour
{

    [Range(0.00f, 1.00f)]
    public float masterVol;

    public List<soundEffect> sfxs;

    public void playSoundEffect(int index)
    {
        soundEffect sEffect = sfxs[index];
        AudioSource sEffectObj = sEffect.returnVariation();
        sEffectObj.volume = masterVol;
        sEffectObj.Play();
    }

}
