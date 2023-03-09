using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseScript : MonoBehaviour
{

    public GameObject objToPulse;
    public float min;
    public float max;
    public float speed;

    private void Update()
    {
        Color newCol = new Color(1, 1, 1, 1);
        float newAlpha = min + Mathf.Abs(max * Mathf.Sin(speed * Time.time)); // |Asin(BX)| + C where A = max, B = speed, and C = minimum
        if(newAlpha > 1) { newAlpha = 1; }
        newCol.a = newAlpha;
        objToPulse.GetComponent<SpriteRenderer>().color = newCol;
    }

}
