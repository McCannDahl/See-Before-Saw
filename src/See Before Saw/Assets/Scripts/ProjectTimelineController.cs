using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTimelineController : MonoBehaviour
{
    public GameObject statusText;
    public void ToggleStatusText()
    {
        var script = statusText.GetComponent<TextMesh>();
        if (script.text == "On Schedule")
        {
            script.text = "Ahead of Schedule";
            script.color = new Color(0,0.4f,0);
        } else if (script.text == "Ahead of Schedule")
        {
            script.text = "Behind Schedule";
            script.color = Color.red;
        } else if (script.text == "Behind Schedule")
        {
            script.text = "On Schedule";
            script.color = Color.white;
        }
    }
    void Start()
    {

    }
    void Awake()
    {

    }
}
