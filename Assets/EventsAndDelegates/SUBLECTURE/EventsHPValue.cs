using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventsHPValue : MonoBehaviour
{
    public TextMeshProUGUI hpValue;

    public void SetHPValueToZero(GameObject killedObject)
    {
        hpValue.text = "0";
    }

    private void OnEnable()
    {
        EventsDelegatesLesson.OnJimmyKilled += SetHPValueToZero;
    }

    private void OnDisable()
    {
        EventsDelegatesLesson.OnJimmyKilled -= SetHPValueToZero;
    }
}
