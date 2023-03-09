using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRocketScript : MonoBehaviour
{

    public menuRocketData menuRocketData;
    public Sprite locked;
    public Sprite unlocked;

    void Update()
    {
        Color newColor = new Color(1, 1, 1, 1);
        if (gameObject.transform.position.x <= -23 || gameObject.transform.position.x >= 23)
        {
            newColor.a = 0;
        }
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
        if(menuRocketData.owned)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = unlocked;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = locked;
        }
    }

}
