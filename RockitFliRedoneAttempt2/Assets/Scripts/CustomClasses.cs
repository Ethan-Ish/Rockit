using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class menuRocketData
{
    [SerializeReference]
    public float price;
    [SerializeReference]
    public string name;
    [SerializeReference]
    public string description;
    [SerializeReference]
    public bool owned;
    [SerializeReference]
    public GameObject rocketPlayer;
    //[SerializeReference]
    //public float defaultSpeed;
}

[System.Serializable]
public class saveDataClass
{
    [SerializeReference]
    public float money;
    [SerializeReference]
    public bool[] unlocked = new bool[] { };
    [SerializeReference]
    public int selectedRocket;
}

[System.Serializable]
public class obstacleClass
{
    [SerializeReference]
    public GameObject gameData;
    [SerializeReference]
    public GameObject particles;
    [SerializeReference]
    public float gravScale;
    [SerializeReference]
    public float angVel;
    [SerializeReference]
    public float rot;
    [SerializeReference]
    public float yKillLvl;
    [SerializeReference]
    public SfxManager sfxMan;
}

public class defaultRocketFunc
{

    public float setSpeed(defaultRocketPlayerData data)
    {
        return data.defaultSpeed;
    }

    public void flyForward(defaultRocketPlayerData data)
    {
        data.rocket.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); // Zeros out velocity
        float x = data.handling;
        x *= Input.GetAxisRaw("Horizontal");
        float y = data.handling;
        y *= Input.GetAxisRaw("Vertical");
        Vector2 newVel = new Vector2(x, y);
        newVel *= Time.fixedDeltaTime;
        newVel *= 100;
        GameData gData = data.gameData.GetComponent<GameData>();
        if(!gData.flying) { newVel *= 0; } // If the rocket isn't supposed to be flying, then it stops
        data.rocket.GetComponent<Rigidbody2D>().velocity = newVel; // Sets new velocity
    }

    public float increaseSpeed(defaultRocketPlayerData data)
    {
        GameData gData = data.gameData.GetComponent<GameData>();
        float newSpeed = data.speed;
        if(!gData.flying) { return newSpeed; }
        newSpeed *= data.speedIncreaseValue;
        newSpeed *= Time.fixedDeltaTime;
        newSpeed *= 0.01f;
        newSpeed += data.speed; // Adds increased speed to original speed to return final exponential growth
        if(newSpeed >= 80)
        {
            newSpeed = 80;
        }
        return newSpeed;
    }

    public void setAllFlames(defaultRocketPlayerData data, bool activeInactive)
    {
        foreach (GameObject flame in data.flames)
        {
            flame.SetActive(activeInactive);
        }
    }

    public void checkDeath(defaultRocketPlayerData data)
    {
        float health = data.gameData.GetComponent<GameData>().rocketHealth;
        GameData gData = data.gameData.GetComponent<GameData>();
        if ((health <= 0) && gData.flying)
        {
            setAllFlames(data, false);
            DeathSequence deathSequence = gData.deathSequence.GetComponent<DeathSequence>(); // Gets death sequence function
            gData.flying = false;
            deathSequence.deathSequence();
        }
    }

    public void updateGameDataStart(defaultRocketPlayerData data)
    {
        data.gameData.GetComponent<GameData>().Speed = data.defaultSpeed;
    }

    public void updateGameDataSpeed(defaultRocketPlayerData data)
    {
        data.gameData.GetComponent<GameData>().Speed = data.speed;
    }

    public void defaultStart(defaultRocketPlayerData data)
    {
        setAllFlames(data, true);
        updateGameDataStart(data);
    }

    public void defaultFrame(defaultRocketPlayerData data)
    {
        checkDeath(data);
    }

    public void defaultFixedFrame(defaultRocketPlayerData data)
    {
        updateGameDataSpeed(data);
        flyForward(data);
    }

    public void updateBar(float max, float cur, Image meter, float defaultWidth)
    {
        float fraction = cur;
        if(!(max == 0))
        {
            fraction /= max;
        }
        fraction *= defaultWidth;
        Rect newRect = meter.rectTransform.rect;
        newRect.width = fraction;
        Vector2 newSizeDelta;
        newSizeDelta.x = newRect.width;
        newSizeDelta.y = newRect.height;
        meter.gameObject.GetComponent<RectTransform>().sizeDelta = newSizeDelta;
    }
}

[System.Serializable]
public class defaultRocketPlayerData
{
    [SerializeReference]
    public GameObject rocket;
    [SerializeReference]
    public float maxHealth;
    [SerializeReference]
    public float speed;
    [SerializeReference]
    public float speedIncreaseValue;
    [SerializeReference]
    public float handling;
    [SerializeReference]
    public List<GameObject> flames;
    [SerializeReference]
    public GameObject gameData;
    [SerializeReference]
    public float defaultSpeed;
}

public class positionFunc
{
    public float distanceCalculator3D(Vector3 pos1, Vector3 pos2)
    {
        float xInterval = pos1.x - pos2.x;
        float yInterval = pos1.y - pos2.y;
        float zInterval = pos1.z - pos2.z;
        xInterval = Mathf.Pow(xInterval, 2);
        yInterval = Mathf.Pow(yInterval, 2);
        zInterval = Mathf.Pow(zInterval, 2);
        float interval = xInterval + yInterval + zInterval;
        float distance = Mathf.Sqrt(interval);
        return distance;
    }

    public float distanceCalculator2D(Vector2 pos1, Vector2 pos2)
    {
        float xInterval = pos1.x - pos2.x;
        float yInterval = pos1.y - pos2.y;
        xInterval = Mathf.Pow(xInterval, 2);
        yInterval = Mathf.Pow(yInterval, 2);
        float interval = xInterval + yInterval;
        float distance = Mathf.Sqrt(interval);
        return distance;
    }

    public void lerpPos(Transform objToLerpTrans, Vector3 posToLerp, float lerpFactor)
    {
        Vector3 currentPos = objToLerpTrans.position;
        Vector3 newGoalPos = posToLerp;
        newGoalPos.z = currentPos.z;
        float lerpA = lerpFactor * Time.deltaTime * 10;
        Vector3 newPos = Vector3.Lerp(currentPos, newGoalPos, lerpA);
        float dist = distanceCalculator3D(newPos, posToLerp);
        if(dist <= 0.1)
        {
            newPos = posToLerp;
        }
        objToLerpTrans.position = newPos;
    }

    public Vector3 getPosBetweenCorners(Vector3 corner1, Vector3 corner2)
    {
        float newX = Random.Range(corner1.x * 100, corner2.x * 100);
        newX *= 0.01f; // brings the 100 range extender back down to a decimal value
        float newY = Random.Range(corner1.y * 100, corner2.y * 100);
        newY *= 0.01f; // brings the 100 range extender back down to a decimal value
        float newZ = Random.Range(corner1.z * 100, corner2.z * 100);
        newZ *= 0.01f; // brings the 100 range extender back down to a decimal value
        Vector3 output;
        output.x = newX;
        output.y = newY;
        output.z = newZ;
        return output;
    }

}

public class customMath
{

    public float rangeWithDec(float min, float max, float dec)
    {
        float fulDec = 1 / dec;
        float ranNum = Random.Range(min * fulDec, max * fulDec);
        ranNum *= dec;
        return ranNum;
    }

    public bool isFullNumber(float input)
    {
        int roundedInput = Mathf.RoundToInt(input);
        bool returnBool = (input == roundedInput); // If rounding the input is the same as the input itself
        return returnBool;
    }

}

[System.Serializable]
public class bulletClass
{
    [SerializeReference]
    public GameObject particles;
    [SerializeReference]
    public MenuManager menuMan;
    [SerializeReference]
    public int sellPrice;
    [SerializeReference]
    public ParticleGenerator pGen;
    [SerializeReference]
    public float verticalVelocity;
    [SerializeReference]
    public SfxManager sfxMan;
}

[System.Serializable]
public class laserClass
{
    [SerializeReference]
    public GameObject redParticles;
    [SerializeReference]
    public float constant;
    [SerializeReference]
    public float speedConstant;
    [SerializeReference]
    public float minimum;
    [SerializeReference]
    public ParticleGenerator pGen;
    [SerializeReference]
    public GameData gData;
}

[System.Serializable]
public class soundEffect
{
    [SerializeReference]
    public List<AudioSource> soundVariations;

    public AudioSource returnVariation()
    {
        int maxIndex = soundVariations.Count;
        int i = Random.Range(0, maxIndex);
        return soundVariations[i];
    }
}