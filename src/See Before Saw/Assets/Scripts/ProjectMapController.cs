using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectMapController : MonoBehaviour
{
    public GameObject slider2;
    public GameObject slider3;
    public GameObject project;
    ProjectController p;
    private void Awake()
    {
        p = project.GetComponent<ProjectController>();
    }
    public void sliderUpdated(GameObject slider)
    {
        PinchSlider ps = slider.GetComponent<PinchSlider>();
        if (ps.SliderValue == 1)
        {
            if (slider == slider2)
            {
                p.CompleteTask(ProjectTasks.shipMaterials_Dealer);
            }
            if (slider == slider3)
            {
                p.CompleteTask(ProjectTasks.shipMaterials_Mills);
            }
        }
    }
}
