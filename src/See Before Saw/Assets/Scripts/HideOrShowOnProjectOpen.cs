using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrShowOnProjectOpen : HideOrShowOnSomething
{
    public GameObject project;
    void Awake()
    {
        EventPublisher.i.HouseGrabbed += ShowProject;
        EventPublisher.i.CloseProjectButtonPressed += HideProject;
        EventPublisher.i.ParivedaLogoPressed += () => ShowerAndHider.hide(gameObject);
    }
    void ShowProject(GameObject p)
    {
        if (p == project)
        {
            Show();
        }
    }
    void HideProject(GameObject p)
    {
        if (p == project)
        {
            Hide();
        }
    }
}
