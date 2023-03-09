using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{

    public AudioSource menuMusic;
    public AudioSource helpMusic;
    public List<AudioSource> rocketMusic;

    [Range(0.00f, 1.00f)]
    public float masterVol;

    [Header("For volume transitions:")]

    public GameObject cam;
    public MenuManager menuMan;

    public float helpPosition;
    public float gamePosition;
    public float menuPosition;

    public void zeroAllVol()
    {
        menuMusic.volume = 0;
        helpMusic.volume = 0;
        foreach(AudioSource aSrc in rocketMusic)
        {
            aSrc.volume = 0;
        }
    }

    public void multMaster(AudioSource aSrc)
    {
        aSrc.volume *= masterVol;
    }

    public void Start()
    {
        zeroAllVol();
    }

    private void Update()
    {
        if(cam.transform.position.x <= 0)
        {
            float progress = cam.transform.position.x;
            progress = progress / helpPosition; // progress = 1 at help page pos, progress = 0 at main menu pos
            helpMusic.volume = progress;
            menuMusic.volume = 1 - progress; // volume = 1 when progress = 0, volume = 0 when progress at 1
            multMaster(helpMusic); // Allows a master volume control
            multMaster(menuMusic);
        }
        else
        {
            int currRock = menuMan.rocketSelected;
            float progress = cam.transform.position.x;
            progress = progress / gamePosition; // progress = 1 at game pos, progress = 0 at main menu pos
            rocketMusic[currRock].volume = progress;
            menuMusic.volume = 1 - progress; // volume = 1 when progress = 0, volume = 0 when progress at 1
            multMaster(rocketMusic[currRock]);
            multMaster(menuMusic);
        }
    }

}
