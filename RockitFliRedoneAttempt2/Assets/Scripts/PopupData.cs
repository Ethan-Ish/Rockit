using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupData : MonoBehaviour
{

    public GameObject popup;
    public Text popupTextObj;

    public void enablePopup(GameObject inputObj)
    {
        GameObject uiObj = inputObj;
        RectTransform trans = uiObj.GetComponent<RectTransform>();
        Rect transRect = trans.rect;
        float offset = transRect.height / 2;
        offset += 0.5f;
        if(uiObj.transform.position.y >= 0)
        {
            offset *= -1f;
        }
        Vector3 pos = uiObj.transform.position;
        pos.z = popup.transform.position.z;
        pos.y += offset;
        popup.transform.position = pos;
        popupTextObj.text = uiObj.name;
        popup.SetActive(true);
    }

    public void disablePopup()
    {
        popup.SetActive(false);
    }

}
