using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWhenSliderFull : MonoBehaviour
{
    public void OnSliderUpdated(SliderEventData data)
    {
        gameObject.SetActive(data.NewValue == 1);
    }
}
