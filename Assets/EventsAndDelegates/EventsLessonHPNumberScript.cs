using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventsLessonHPNumberScript : MonoBehaviour
{
    //6
    public TextMeshProUGUI HPText;

    //2
    /// <summary>
    /// Called when the object is enabled, at the same time as Start
    /// </summary>
    private void OnEnable()
    {
        //4
        //subscribe the SetHealthbarToZero to the OnJohnKilled event
        EventsLessonScript.OnJohnKilled += SetHealthNumberToZero;
    }

    //3
    /// <summary>
    /// Called when the object is disabled or destroyed
    /// </summary>
    private void OnDisable()
    {
        //5
        //unsubs the SetHealthbarToZero function from OnJohnKilled
        EventsLessonScript.OnJohnKilled -= SetHealthNumberToZero;
    }

    //1
    /// <summary>
    /// Sets value of HP text display to "0"
    /// </summary>
    /// <param name="killedObject"></param>
    public void SetHealthNumberToZero(GameObject killedObject)
    {
        //7
        HPText.text = "0";
    }
}
