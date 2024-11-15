using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsHPBar : MonoBehaviour
{
    public Slider healthbarSlider;

    public void SetHealthbarToZero(GameObject killedObject)
    {
        healthbarSlider.normalizedValue = 0;
    }

    private void OnEnable()
    {
        EventsDelegatesLesson.OnJimmyKilled += SetHealthbarToZero;
    }

    private void OnDisable()
    {
        EventsDelegatesLesson.OnJimmyKilled -= SetHealthbarToZero;
    }
}
