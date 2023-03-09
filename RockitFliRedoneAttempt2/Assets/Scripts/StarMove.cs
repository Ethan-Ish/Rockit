using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMove : MonoBehaviour
{

    public float speed;
    public List<GameObject> stars;

    private float distance;
    private float topPos;

    private void Start()
    {
        distance = stars[1].transform.position.y - stars[0].transform.position.y;
        topPos = stars[stars.Count - 1].transform.position.y;
    }

    private void FixedUpdate()
    {
        foreach(GameObject star in stars)
        {
            Vector3 newPos = new Vector3();
            newPos = star.transform.position;
            newPos.y += (-1f * speed * Time.fixedDeltaTime * 3);
            if (star.transform.position.y <= (-2f * distance))
            {
                newPos.y = topPos;
            }
            star.transform.position = newPos;
        }
    }

}
