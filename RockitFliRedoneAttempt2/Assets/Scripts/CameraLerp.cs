using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    public float goalX;
    public float lerpFactor;

    private positionFunc posFunc = new positionFunc();

    private void Update()
    {
        Vector3 lerpPos = gameObject.transform.position;
        lerpPos.x = goalX;
        posFunc.lerpPos(gameObject.transform, lerpPos, lerpFactor);
    }

    public void moveCamera(float x)
    {
        goalX = x;
    }

}
