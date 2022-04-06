using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSliderBetweenTwoPoints : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    Transform trackVisuals;
    Transform ThumbRoot;
    PinchSlider pinchSlider;
    private void Awake()
    {
        trackVisuals = transform.Find("TrackVisuals");
        ThumbRoot = transform.Find("ThumbRoot");
        pinchSlider = gameObject.GetComponent<PinchSlider>();
        pinchSlider.SliderValue = 0;
    }

    void Update()
    {
        updateSliderTransform();
    }

    private void updateSliderTransform()
    {
        // move into position
        transform.position = Vector3.Lerp(a.transform.position, b.transform.position, 0.5f);
        // have the right rotation
        var dir = a.transform.position - b.transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        // have the right length
        var dist = Vector3.Distance(a.transform.position, b.transform.position);
        trackVisuals.localScale = new Vector3(dist * 4f, trackVisuals.localScale.y, trackVisuals.localScale.z);
        // adjust ends
        pinchSlider.SliderEndDistance = dist / 2 - 0.02f;
        pinchSlider.SliderStartDistance = -pinchSlider.SliderEndDistance;
        // adjust truck to end
        if (pinchSlider.SliderValue == 0)
        {
            ThumbRoot.position = pinchSlider.SliderStartPosition;
        }
    }
}
