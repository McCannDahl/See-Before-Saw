using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEventSetSliderTo1 : MonoBehaviour
{
    PinchSlider ps;
    public List<EventPublisherEvents> events;
    void Start()
    {
        ps = GetComponent<PinchSlider>();
        foreach(var e in events)
        {
            EventPublisher.i.AddListener(e,SetSliderTo1);
        }
    }

    private void SetSliderTo1()
    {
        ps.SliderValue = 1;
    }
}
