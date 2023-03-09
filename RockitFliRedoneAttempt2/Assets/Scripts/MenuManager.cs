using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public float Money;
    public int rocketSelected;

    public menuRocketData currentRocketData;

    [Header("Objects")]

    public Text buyButtonText;
    public Button buyButton;
    public Button playButton;
    public List<Text> moneyDisplays;
    public CameraLerp cameraLerpScript;
    public GameObject deathScreen1;
    public GameObject deathScreen2;
    public GameObject gameData;
    public MusicManager musicManager;
    public Text meterDisplay;

    public List<GameObject> menuRockets = new List<GameObject>();
    private positionFunc posFunction = new positionFunc();
    private SaveDataManager saveDataManager = new SaveDataManager();

    public void updateRocketList()
    {
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            Transform rocketTrans = gameObject.transform.GetChild(i);
            GameObject rocketGameObj = rocketTrans.gameObject;
            menuRockets.Add(rocketGameObj);
            rocketGameObj.SetActive(true);
        }
    }

    public void updateBuyButtonText()
    {
        string title = "";
        string description = "";
        string finalText = "";
        title = currentRocketData.name;
        title += " - ";
        if (currentRocketData.owned)
        {
            title += "Owned";
        }
        else if (currentRocketData.price == 0)
        {
            title += "Free";
        }
        else
        {
            title += "$" + currentRocketData.price.ToString();
        }
        description += currentRocketData.description;
        finalText = title + "\n" + description;
        buyButtonText.text = finalText;
    }

    public float getRocketSelectPos(GameObject rocket)
    {
        float x = rocket.transform.localPosition.x * -1f;
        return x;
    }

    public void nextRocket()
    {
        float newIndex = rocketSelected + 1;
        if (newIndex > (menuRockets.Count - 1))
        {
            newIndex = 0;
        }
        rocketSelected = Mathf.RoundToInt(newIndex);
    }

    public void prevRocket()
    {
        float newIndex = rocketSelected - 1;
        if (newIndex < 0)
        {
            newIndex = (menuRockets.Count - 1);
        }
        rocketSelected = Mathf.RoundToInt(newIndex);
    }

    public void updateMeterDisplay()
    {
        GameData gData = gameData.GetComponent<GameData>();
        string newText = gData.meters.ToString() + "km";
        meterDisplay.text = newText;
    }

    public void gameStart()
    {
        deathScreen1.SetActive(false);
        deathScreen2.SetActive(false);
        GameData gData = gameData.GetComponent<GameData>();
        gData.count = 0; // resets the distance timer
        gData.meters = 0;
        gData.flying = true;
        ObstacleManager obsMan = gData.obsManager.GetComponent<ObstacleManager>();
        obsMan.deleteAllObstacles();
        obsMan.spawn = true;
        GameObject currRocket = currentRocketData.rocketPlayer;
        gData.rocketMaxHealth = currRocket.GetComponent<DefaultRocketScript>().rPD.maxHealth;
        gData.rocketHealth = currRocket.GetComponent<DefaultRocketScript>().rPD.maxHealth;
        currRocket.SetActive(true);
        musicManager.zeroAllVol();
        cameraLerpScript.moveCamera(46);
    }

    public void buyRocket()
    {
        GameObject menuRocket = menuRockets[rocketSelected];
        currentRocketData = menuRocket.GetComponent<MenuRocketScript>().menuRocketData;
        if ((Money >= currentRocketData.price) && !(currentRocketData.owned))
        {
            Money -= currentRocketData.price;
            menuRocket.GetComponent<MenuRocketScript>().menuRocketData.owned = true;
        }
    }

    public void checkBuyButton()
    {
        buyButton.interactable = (Money >= currentRocketData.price) || currentRocketData.owned;
    }

    public void updateMoneyDisplay()
    {
        string moneyDisplayText = "$";
        moneyDisplayText += Money.ToString();
        foreach (Text moneyDisplay in moneyDisplays)
        {
            moneyDisplay.text = moneyDisplayText;
        }
    }

    public void loadSaveData()
    {
        saveDataClass saveData = saveDataManager.Load();
        Money = saveData.money;
        rocketSelected = saveData.selectedRocket;
        int i = 0;
        foreach (GameObject rocket in menuRockets)
        {
            bool unlocked;
            if (saveData.unlocked.Length >= i)
            {
                unlocked = saveData.unlocked[i];
            }
            else
            {
                unlocked = false;
            }
            menuRockets[i].GetComponent<MenuRocketScript>().menuRocketData.owned = unlocked;
            i += 1;
        }
    }

    public void saveData()
    {
        saveDataClass saveData = new saveDataClass();
        saveData.money = Money;
        saveData.selectedRocket = rocketSelected;
        saveData.unlocked = new bool[menuRockets.Count];
        int i = 0;
        foreach (GameObject rocket in menuRockets)
        {
            bool unlocked = menuRockets[i].GetComponent<MenuRocketScript>().menuRocketData.owned;
            saveData.unlocked[i] = unlocked;
            i += 1;
        }
        saveDataManager.Save(saveData);
        saveDataManager.printFilePath();
    }

    public void updatePlayButton()
    {
        string newName;
        playButton.interactable = currentRocketData.owned;
        if(currentRocketData.owned) { newName = "Play"; }
        else { newName = "Rocket unowned!"; }
        playButton.name = newName;
    }

    private void Start()
    {
        updateRocketList();
        loadSaveData();
    }

    private void Update()
    {
        GameObject menuRocket = menuRockets[rocketSelected];
        currentRocketData = menuRocket.GetComponent<MenuRocketScript>().menuRocketData;
        Vector3 newPos = gameObject.transform.position;
        newPos.x = getRocketSelectPos(menuRocket);
        updateBuyButtonText();
        checkBuyButton();
        updateMoneyDisplay();
        updateMeterDisplay();
        updatePlayButton();
        posFunction.lerpPos(gameObject.transform, newPos, 3f);
    }

}