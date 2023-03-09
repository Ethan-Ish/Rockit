using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    public obstacleClass defaultObstacleData;
    public float minGravityScaleRange;
    public float maxGravityScaleRange;
    public Transform spawnedObsFol;
    public Transform obsTemplateFol;
    public Vector3 centerPos;
    public float range;
    public bool spawn;
    public float obsCounter;

    private List<GameObject> objectList = new List<GameObject>();

    private void Start()
    {
        objectList = collectAllChildren("Obstacle");
    }

    public void createObstacle(GameObject obj, Vector3 pos, float maxScale, float minScale, Transform newFol)
    {
        GameObject newObst = GameObject.Instantiate(obj);
        newObst.SetActive(false);
        newObst.transform.parent = newFol;
        newObst.transform.position = pos;
        newObst.AddComponent<ObstacleScript>();
        newObst.GetComponent<ObstacleScript>().obData = defaultObstacleData;
        float gravScale;
        float constant = 100;
        gravScale = Random.Range(minScale * constant, maxScale * constant);
        gravScale /= constant;
        newObst.GetComponent<ObstacleScript>().obData.gravScale = gravScale;
        float rotationForce; // Generates a random angular velocity to make the obstacles spin
        rotationForce = Random.Range(-100f, 100f);
        rotationForce *= 0.01f; // (in case it's unclear, this converts the -100-100 range to a decimal between -1 and 1)
        rotationForce *= 100;
        float speed = defaultObstacleData.gameData.GetComponent<GameData>().Speed;
        rotationForce *= speed;
        newObst.GetComponent<ObstacleScript>().obData.angVel = rotationForce;
        float rotation; // Generates a random starting rotation
        rotation = Random.Range(0, 360);
        newObst.GetComponent<ObstacleScript>().obData.rot = rotation;
        newObst.SetActive(true);
        newObst.name = "Obstacle (made " + Time.time.ToString() + ")";
    }

    public float createXWithRange()
    {
        float returnX;
        float centerX = centerPos.x;
        returnX = centerX;
        float negPositive = Random.Range(-100f, 100f); // Generates a neg/positive portion of the range
        negPositive *= 0.01f;
        float rangeAdd = negPositive * range; // Add / subtract a portion of the max range
        returnX += rangeAdd;
        return returnX;
    }

    public List<GameObject> collectAllChildren(string tag)
    {
        List<GameObject> returnList = new List<GameObject>();
        for (int i = 0; i < obsTemplateFol.childCount; ++i)
        {
            Transform obsTrans = obsTemplateFol.GetChild(i);
            GameObject obsTransObj = obsTrans.gameObject;
            if(obsTransObj.tag == tag)
            {
                returnList.Add(obsTransObj);
            }
        }
        return returnList;
    }

    public void spawnNewObstacle()
    {
        // Get Obstacle Template
        GameObject obsTemplate;
        int index;
        index = Mathf.RoundToInt(Random.Range(0, objectList.Count - 1));
        obsTemplate = objectList[index];
        // Get New Obstacle Position
        Vector3 obsPos = centerPos;
        obsPos.x = createXWithRange();
        // Spawn Obstacle
        createObstacle(obsTemplate, obsPos, maxGravityScaleRange, minGravityScaleRange, spawnedObsFol);
    }

    public void deleteAllObstacles()
    {
        for (int i = (spawnedObsFol.childCount - 1); i >= 0;)
        {
            if(i <= (spawnedObsFol.childCount - 1) && i >= 0)
            {
                Transform obsTrans = spawnedObsFol.GetChild(i);
                GameObject obsTransObj = obsTrans.gameObject;
                GameObject.Destroy(obsTransObj);
                i -= 1;
            }
        }
        obsCounter = 0; // resets obstacle timer
    }

    public float waitDecrease(float input, int formula)
    {
        float returnFloat = input;
        if(formula == 1)
        {
            returnFloat = 30;
            returnFloat /= (input + 5);
            returnFloat -= 30 / 105;
        }
        if(formula == 2)
        {
            returnFloat = input;
            returnFloat *= -0.01f;
            returnFloat += 1;
        }
        return returnFloat;
    }

    private void FixedUpdate()
    {
        obsCounter += Time.fixedDeltaTime;
        float speed = defaultObstacleData.gameData.GetComponent<GameData>().Speed;
        if (obsCounter >= waitDecrease(speed, 2))
        {
            obsCounter = 0;
            if(spawn) { spawnNewObstacle(); }
        }
    }

}
