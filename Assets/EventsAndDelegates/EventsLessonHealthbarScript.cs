using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsLessonHealthbarScript : MonoBehaviour
{
    //6
    public Slider healthBarSlider;

    //2
    /// <summary>
    /// Called when the object is enabled, at the same time as Start
    /// </summary>
    private void OnEnable()
    {
        //4
        //subscribe the SetHealthbarToZero to the OnJohnKilled event
        EventsLessonScript.OnJohnKilled += SetHealthbarToZero;
    }

    //3
    /// <summary>
    /// Called when the object is disabled or destroyed
    /// </summary>
    private void OnDisable()
    {
        //5
        //unsubs the SetHealthbarToZero function from OnJohnKilled
        EventsLessonScript.OnJohnKilled -= SetHealthbarToZero;
    }

    //1
    /// <summary>
    /// sets normalized value of slider to 0
    /// </summary>
    /// <param name="killedObject"></param>
    public void SetHealthbarToZero(GameObject killedObject)
	{
        //7
        healthBarSlider.normalizedValue = 0;
	}
}
