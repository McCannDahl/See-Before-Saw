using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPercentOfChildrenWhenSliderUpdated : MonoBehaviour
{
    public void Awake()
    {
        EventPublisher.i.DesignButtonPressed += (_) => OnSliderUpdated(0.999f);
        EventPublisher.i.StartOptimization += () => OnSliderUpdated(0.999f);
    }
    public void OnSliderUpdated(SliderEventData data)
    {
        if (data.OldValue != data.NewValue)
        {
            OnSliderUpdated(data.NewValue);
        }
    }
    public void OnSliderUpdated(float NewValue)
    {
        if (NewValue == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            var actualFrame = transform.GetChild(0);
            for (int i = 0; i < actualFrame.childCount; i++)
            {
                bool shouldShow = i < actualFrame.childCount * NewValue;
                GameObject child = actualFrame.GetChild(i).gameObject;
                if (shouldShow)
                {
                    ShowerAndHider.show(child);
                }
                else
                {
                    ShowerAndHider.hide(child);
                }
            }
        }
    }
}
