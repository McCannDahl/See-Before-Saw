using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnSliderEvent : MonoBehaviour
{
    private Vector3 origionalScale;
    public float fullSizeScale = 1f;

    void Awake()
    {
        EventPublisher.i.DesignMenuSliderUpdated += UpdateScale;
        origionalScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    private void UpdateScale(SliderEventData data)
    {
        if (origionalScale != Vector3.zero)
        {
            transform.localScale = new Vector3(origionalScale.x + data.NewValue * (fullSizeScale - origionalScale.x), origionalScale.y + data.NewValue * (fullSizeScale - origionalScale.y), origionalScale.z + data.NewValue * (fullSizeScale - origionalScale.z));
        }
    }
}
