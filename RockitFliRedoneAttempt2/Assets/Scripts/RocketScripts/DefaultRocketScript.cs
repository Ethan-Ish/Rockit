using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultRocketScript : MonoBehaviour
{

    public defaultRocketPlayerData rPD;

    private defaultRocketFunc rFunc = new defaultRocketFunc();

    private void OnEnable()
    {
        rFunc.defaultStart(rPD);
        rPD.speed = rFunc.setSpeed(rPD);
    }

    private void Update()
    {
        rFunc.defaultFrame(rPD);
    }

    private void FixedUpdate()
    {
        rPD.speed = rFunc.increaseSpeed(rPD);
        rFunc.defaultFixedFrame(rPD);
    }

}
